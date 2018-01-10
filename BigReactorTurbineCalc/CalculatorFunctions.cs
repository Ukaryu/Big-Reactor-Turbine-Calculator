using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;

namespace BigReactorTurbineCalc
{
    class CalculatorFunctions
    {

        public ObservableCollection<Material> materialsList = new ObservableCollection<Material>();

        public ObservableCollection<TurbineBuild> mostEfficientBuilds { get; set; }

        public Material selectedMaterial { get; set; }

        private double steamIn { get; set; }
        private double rotorSpeed { get; set; }
        private double coilBlocks { get; set; }
        private double bladeCount { get; set; }
        private double shaftCount { get; set; }

        public CalculatorFunctions ()
        {
            LoadJson();
            selectedMaterial = new Material { name = "", dragCoefficient = 0, efficiency = 0, energyBonus = 0 };
            steamIn = 500;
            rotorSpeed = 800;
            coilBlocks = 8;
            bladeCount = 4;
            shaftCount = 2;
            mostEfficientBuilds = new ObservableCollection<TurbineBuild>();

        }

        public double calcGeneratedEnergy()
        {
            double generatedEnergy = selectedMaterial.efficiency * calcTurbineEfficiency();
            generatedEnergy *= Math.Pow((coilBlocks * selectedMaterial.dragCoefficient * calcRotorSpeed()), selectedMaterial.energyBonus);
            return generatedEnergy;
        }

        public double calcTurbineEfficiency()
        {
            double rotorSpeed = calcRotorSpeed();
            if (rotorSpeed >= 500)
            {
                return (0.25 * (Math.Cos(rotorSpeed / (45.5 * Math.PI)) + 3) );

            } else if(rotorSpeed < 500)
            {
                return 0.5;

            } else
            {
                return 0;
            }
        }

        public double calcRotorSpeed()
        {
            return calcRotorEnergy() / (bladeCount * calcRotorMass());
        }

        public double calcRotorEnergy()
        {
            double a = 1 - calcInductionTorque() - (1 / (4000 * calcRotorMass()));
            double b = calcLiftTorque() - (calcRotorMass() / 10);
            return b / (1 - a);
        }
        //Required Steam
        //public double calcRequiredSteam()
        //{

        //    return 0;
        //}

        //public double calcRequiredRotorEnergy()
        //{
        //    return rotorSpeed * bladeCount * calcRotorMass();
        //}

        //public double calcB()
        //{
        //    double a = 1 - calcInductionTorque() - (1 / (4000 * calcRotorMass()));
        //    return calcRequiredRotorEnergy() - (calcRequiredRotorEnergy() * a);
        //}

        //public double calcOldLiftTorque()
        //{
        //    return calcB() + (calcRotorMass() / 10);
        //}

        //public double calcSteamIn()
        //{
        //    double steamIn = 25 * bladeCount * calcUseableSteamEnergyBC();
        //    steamIn /= (-calcOldLiftTorque() + 250 * bladeCount + calcUseableSteamEnergyBC());
        //    return steamIn;
        //}

        //public double calcUseableSteamEnergyBC()
        //{
        //    return 10 * Math.Min((25 * bladeCount), 2000);
        //}
        //Required Steam

        public double calcInductionTorque()
        {
            return (coilBlocks * selectedMaterial.dragCoefficient) / (bladeCount * calcRotorMass());
        }

        public double calcLiftTorque()
        {
            double liftTorque = bladeCount * (10 * steamIn - calcUseableSteamEnergy());
            liftTorque /= (steamIn / 25);
            return liftTorque + calcUseableSteamEnergy();
        }
        
        public double calcUseableSteamEnergy()
        {
            return 10 * Math.Min((25 * bladeCount), steamIn);
        }

        public double calcRotorMass()
        {
            return (8 + bladeCount + shaftCount) * 10;
        }

        public void calcMostEfficientBuilds()
        {
            mostEfficientBuilds.Clear();
            
            for (var i = 0; i < 100; i++)
            {
                shaftCount = i;
                bladeCount = 4 * (i - 1);
                if (calcTurbineEfficiency() >= 0.99)
                {
                    if((calcRotorSpeed() > 700 || calcRotorSpeed() > 1700) && (calcRotorSpeed() < 900 || calcRotorSpeed() < 1900))
                    {
                        mostEfficientBuilds.Add(new TurbineBuild
                        {
                            coilBlocks = this.coilBlocks,
                            bladeCount = this.bladeCount,
                            shaftCount = this.shaftCount,
                            efficiency = calcTurbineEfficiency(),
                            energyGeneration = calcGeneratedEnergy(),
                            steamIn = this.steamIn,
                            rotorSpeed = calcRotorSpeed()
                        });
                    } 

                }
            }

        }

        public void LoadJson()
        {
            using (StreamReader r = new StreamReader("materials.json"))
            {
                string json = r.ReadToEnd();
                materialsList = JsonConvert.DeserializeObject<ObservableCollection<Material>>(json);
            }
        }

    }

    public class TurbineBuild
    {
        public double efficiency { get; set; }
        public double energyGeneration { get; set; }
        public double steamIn { get; set; }
        public double bladeCount { get; set; }
        public double shaftCount { get; set; }
        public double coilBlocks { get; set; }
        public double rotorSpeed { get; set; }
    }
    public class Material
    {
        [JsonProperty("Material Name")]
        public string name { get; set; }
        [JsonProperty("Drag Coefficient")]
        public double dragCoefficient { get; set; }
        [JsonProperty("Energy Bonus")]
        public double energyBonus { get; set; }
        [JsonProperty("Efficiency")]
        public double efficiency { get; set; }

    }

    
}
