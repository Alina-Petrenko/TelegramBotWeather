// <copyright file="Forecast.cs" company="AlinaP">
// Copyright (c) AlinaP. All rights reserved.
// </copyright>

namespace TelegramBotWeather.Models
{
    /// <summary>
    /// Represents the forecasted weather information for a specific time.
    /// </summary>
    public class Forecast
    {
        /// <summary>
        /// Gets or sets the main weather parameters for the forecast.
        /// </summary>
        public Main Main { get; set; }

        /// <summary>
        /// Gets or sets an array of weather conditions associated with the forecast.
        /// </summary>
        public Weather[] Weather { get; set; }
    }
}