using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Test_Taste_Console_Application.Domain.DataTransferObjects;

namespace Test_Taste_Console_Application.Domain.Objects
{
    public class Planet
    {
        private object planetDto;

        public string Id { get; set; }
        public float SemiMajorAxis { get; set; }
        public ICollection<Moon> Moons { get; set; }

        //Set Property added to set Average Moon Gravity
        public float AverageMoonGravity{ get; set; }

        public Planet(PlanetDto planetDto)
        {
            Id = planetDto.Id;
            SemiMajorAxis = planetDto.SemiMajorAxis;
            Moons = new Collection<Moon>();
            float TotalMoonGravity = 0.0f;
            if(planetDto.Moons != null)
            {
                foreach (MoonDto moonDto in planetDto.Moons)
                {
                    Moons.Add(new Moon(moonDto));
                    TotalMoonGravity += moonDto.Gravity;
                }
                //calculating arithmetic average Gravity
                CalAverageMoonGravity();
            }
        }

        //function for calculating arithmetic average of Gravity
        public void CalAverageMoonGravity()
        {
            if (Moons != null)
            {
                float TotalMoonGravity = 0.0f;
                foreach (Moon moon in Moons)
                {
                    TotalMoonGravity += moon.Gravity;
                }
                AverageMoonGravity = TotalMoonGravity / Moons.Count;
            }
        }

        public Boolean HasMoons()
        {
            return (Moons != null && Moons.Count > 0);
        }
    }
}
