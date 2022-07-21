using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application.Domain.Services
{
    class BodiesService
    {
        public class Bodies
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool isPlanet { get; set; }
            public float MassValue { get; set; }
            public float MassExponent { get; set; }
            public float Gravity { get; set; }

            //Added planet property for alternative solution
            //[JsonProperty("aroundPlanet.planet")] public string aroundPlanet { get; set; }
            public string aroundPlanetName { get; set; }

        }
        public class PlanetMoonAvgGravity
        {
            public string PlanetId { get; set; }
            public float MoonAvgGravity { get; set; }
        }
        public IEnumerable<PlanetMoonAvgGravity> GetPlanetMoonAverageGravity()
        {
            List<Bodies> ListBodies = new List<Bodies>();
            //Get Bodies from https://api.le-systeme-solaire.net/rest/bodies
            //Load into ListBodies
            var PlanetMoonAvgGravity = from planet in ListBodies.Where(p => p.isPlanet == true)
                                       join moon in ListBodies.Where(m => m.isPlanet == false).DefaultIfEmpty()
                                       on planet.Name equals moon.aroundPlanetName
                                       group new { planet, moon } by new { planet.Id } into planetGroup
                                       let MoonAvgGravity = planetGroup.Average(a => a.moon.Gravity)
                                       select new PlanetMoonAvgGravity
                                       {
                                           PlanetId = planetGroup.Key.Id,
                                           MoonAvgGravity = MoonAvgGravity
                                       };
            return PlanetMoonAvgGravity;
        }

        //Below function can be move to ScreenOutputService.cs
        public void OutputAllPlanetsAndTheirAverageMoonGravityToConsole()
        {
            //The function works the same way as the PrintAllPlanetsAndTheirMoonsToConsole function. You can find more comments there.
        
            var columnSizes = new[] { 20, 30 };
            var columnLabels = new[]
            {
                OutputString.PlanetId, OutputString.PlanetMoonAverageGravity
            };

            ConsoleWriter.CreateHeader(columnLabels, columnSizes);
            var planets = GetPlanetMoonAverageGravity();
            foreach (PlanetMoonAvgGravity planet in planets)
            {
                ConsoleWriter.CreateText(new string[] { $"{planet.PlanetId}", $"{planet.MoonAvgGravity}" }, columnSizes);
            }

            ConsoleWriter.CreateLine(columnSizes);
            ConsoleWriter.CreateEmptyLines(2);

            /*
                --------------------+--------------------------------------------------
                Planet's Number     |Planet's Average Moon Gravity
                --------------------+--------------------------------------------------
                1                   |0.0f
                --------------------+--------------------------------------------------
            */
        }
    }
}
