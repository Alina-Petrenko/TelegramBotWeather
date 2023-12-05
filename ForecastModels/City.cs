// <copyright file="City.cs" company="AlinaP">
// Copyright (c) AlinaP. All rights reserved.
// </copyright>

namespace TelegramBotWeather.Models
{
    /// <summary>
    /// Represents geographical information about a city.
    /// </summary>
    public class City
    {
        /// <summary>
        /// Gets or sets the name of the city.
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of the city.
        /// </summary>
        public float Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the city.
        /// </summary>
        public float Lon { get; set; }
    }
}