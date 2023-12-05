// <copyright file="UserData.cs" company="AlinaP">
// Copyright (c) AlinaP. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace TelegramBotWeather.CustomModels
{
    /// <summary>
    /// Represents user-specific data for tracking the state and preferences in the Weather bot.
    /// </summary>
    public class UserData
    {
        private static readonly Dictionary<long, UserData> userStates = new Dictionary<long, UserData>();

        /// <summary>
        /// Gets or sets the current state of interaction for the user.
        /// </summary>
        public UserState State { get; set; }

        /// <summary>
        /// Gets or sets the selected city for weather-related queries.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Sets the user state in the dictionary based on the provided chat ID.
        /// </summary>
        /// <param name="chatId">The unique identifier of the chat.</param>
        /// <param name="userData">The user-specific data to be associated with the chat ID.</param>
        public static void SetUserState(long chatId, UserData userData)
        {
            userStates[chatId] = userData;
        }

        /// <summary>
        /// Retrieves the user state based on the provided chat ID.
        /// If the user is not found, a new UserData object with the default state is created and stored.
        /// </summary>
        /// <param name="chatId">The unique identifier of the chat.</param>
        /// <returns>The UserData object associated with the provided chat ID.</returns>
        public static UserData GetUserState(long chatId)
        {
            if (!userStates.TryGetValue(chatId, out var userData))
            {
                userData = new UserData { State = UserState.Default };
                userStates[chatId] = userData;
            }

            return userData;
        }
    }
}