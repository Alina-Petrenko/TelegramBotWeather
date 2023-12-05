// <copyright file="UserState.cs" company="AlinaP">
// Copyright (c) AlinaP. All rights reserved.
// </copyright>

namespace TelegramBotWeather.CustomModels
{
    /// <summary>
    /// Represents the possible states of user interaction in the Weather bot.
    /// </summary>
    public enum UserState
    {
        /// <summary>
        /// The default state indicating no specific action is expected from the user.
        /// </summary>
        Default,

        /// <summary>
        /// Indicates that the bot is currently waiting for the user to provide or select a city.
        /// </summary>
        WaitingForCity
    }
}