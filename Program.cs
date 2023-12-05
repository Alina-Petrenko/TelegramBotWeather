// <copyright file="Program.cs" company="AlinaP">
// Copyright (c) AlinaP. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotWeather.CustomModels;

namespace TelegramBotWeather
{
    internal class Program
    {
        private static ITelegramBotClient botClient;
        private static ReceiverOptions receiverOptions;

        static void Main()
        {
            Console.WriteLine("Write your Telegram token: ");
            var telegramToken = Console.ReadLine();

            botClient = new TelegramBotClient(telegramToken);
            receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                },
                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();

            botClient.StartReceiving(
                UpdateHandlerAsync,
                ErrorHandlerAsync,
                receiverOptions,
                cts.Token);
            Console.ReadLine();
        }

        private static async Task UpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
             {
                var message = update.Message;
                var chatId = message.Chat.Id;
                var userData = UserData.GetUserState(message.Chat.Id);

                if (message == null || message.Type != MessageType.Text)
                    return;

                if (message.Text.StartsWith('/'))
                {
                    await WeatherProvider.GetCommand(
                        botClient,
                        message.Text,
                        chatId,
                        cancellationToken);
                }
                else if (userData.State == UserState.WaitingForCity)
                {
                    await WeatherProvider.GetCommand(
                        botClient,
                        "/setcity",
                        chatId,
                        cancellationToken,
                        message.Text);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId,
                        "You need to set a city at first!",
                        cancellationToken: cancellationToken);
                }
            }
            catch
            {
                await botClient.SendTextMessageAsync(
                    update.Message.Chat.Id, 
                    "Ooops! It's looks like something wrong. Try again!", 
                    cancellationToken: cancellationToken);
            }
        }

        private static Task ErrorHandlerAsync(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}