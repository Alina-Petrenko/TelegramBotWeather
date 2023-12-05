// <copyright file="WeatherProvoder.cs" company="AlinaP">
// Copyright (c) AlinaP. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Threading;
using System;

using TelegramBotWeather.Models;
using Telegram.Bot;
using TelegramBotWeather.CustomModels;

namespace TelegramBotWeather
{
    /// <summary>
    /// Provides weather-related functionality for the Telegram bot, including commands to get weather information for a specified city.
    /// </summary>
    internal class WeatherProvider
    {
        /// <summary>
        /// Processes incoming commands and responds accordingly, handling weather-related requests.
        /// </summary>
        /// <param name="botClient">The Telegram bot client.</param>
        /// <param name="command">The received command.</param>
        /// <param name="chatId">The unique identifier of the chat.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operations.</param>
        /// <param name="message">Additional message content (if any).</param>
        internal static async Task GetCommand(ITelegramBotClient botClient, string command, long chatId, CancellationToken cancellationToken, string message = null)
        {
            switch (command)
            {
                case "/start":
                    {
                        await botClient.SendTextMessageAsync(
                            chatId,
                            "Hi there! I am a Weather bot. I can give you information about the weather in any city! " +
                            "Please, use the command '/setcity' to select a city, and '/getweather' to get the current weather.", 
                            cancellationToken: cancellationToken);
                        break;
                    }
                case "/getweather":
                    {
                        var userData = UserData.GetUserState(chatId);
                        if (userData.City is not null)
                        {
                            var forecast = await GetWeather(userData.City);
                            if (forecast != null)
                            {
                                var result = forecast;
                                var temp = Math.Ceiling(result.Main.Temp).ToString("+#;-#;0");
                                var feelsLikeTemp = Math.Ceiling(result.Main.Temp).ToString("+#;-#;0");

                                await botClient.SendTextMessageAsync(
                                    chatId, 
                                    $"Today in {userData.City} is {result.Weather[0].Description}. Temperature is {temp}°C. " +
                                    $" Feels like {feelsLikeTemp}°C", 
                                    cancellationToken: cancellationToken);
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(
                                    chatId, 
                                    "Make sure you wrote right city!", 
                                    cancellationToken: cancellationToken);
                            }
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId, 
                                "You need to set a city at first!", 
                                cancellationToken: cancellationToken);
                        }
                        break;
                    }
                case "/setcity":
                    {
                        var userData = UserData.GetUserState(chatId);
                        if (userData.State == UserState.WaitingForCity)
                        {
                            await AddCity(botClient, message, chatId);
                        }
                        else
                        {
                            UserData.SetUserState(chatId, new UserData { State = UserState.WaitingForCity });
                            await botClient.SendTextMessageAsync(
                                chatId, 
                                "Write your city", 
                                cancellationToken: cancellationToken);
                        }

                        break;
                    }
                default:
                    {
                        await AddCity(botClient, message, chatId);
                        break;
                    }
            }
        }

        /// <summary>
        /// Retrieves weather information for a specified city.
        /// </summary>
        /// <param name="city">The city for which to retrieve weather information.</param>
        /// <returns>A <see cref="Forecast"/> object representing the weather forecast.</returns>
        private static async Task<Forecast> GetWeather(string city)
        {
            var apiKey = "your api key";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetFromJsonAsync<List<City>>($"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apiKey}");

            if (response != null && response.Count > 0)
            {
                var lat = Math.Round(response[0].Lat, 2);
                var lon = Math.Round(response[0].Lon, 2);
                var path = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={apiKey}";
                var forecast = await httpClient.GetFromJsonAsync<Forecast>(path);
                return forecast;
            }
            return null;
        }

        /// <summary>
        /// Adds a city to the user's preferences based on the received message.
        /// </summary>
        /// <param name="botClient">The Telegram bot client.</param>
        /// <param name="message">The received message containing the city name.</param>
        /// <param name="chatId">The unique identifier of the chat.</param>
        private static async Task AddCity(ITelegramBotClient botClient, string message, long chatId)
        {
            var userData = UserData.GetUserState(chatId);
            if (userData.State == UserState.WaitingForCity)
            {
                userData.City = message;
                await botClient.SendTextMessageAsync(chatId, $"The city successfully added!");
                userData.State = UserState.Default;
            }
        }
    }
}