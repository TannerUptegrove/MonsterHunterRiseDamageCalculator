using System;
using System.Globalization;
using static System.Console;

namespace MHRCalculator
{
    internal class MHRCalculator
    {
        #region Ugly Variable List
        /********************************************************************************************
         * User input variables:
         * As the calculations execute, the variables for raw damage, elemental damage, affinity,
         * and hitzones may be altered. In the case the user wants to redo the calculations, these
         * variables will be used to reset to their origional entered values before any calculations. 
        *********************************************************************************************/
        static int enteredRaw, enteredEle, enteredRawFlatBonusDmg, enteredEleFlatBonusDmg;
        static float enteredAffinity, enteredRawHitzone, enteredEleHitzone;
        static string enteredSharpness;

        //Basic variables
        static bool includeSunbreakArmorSkills;
        static int statusMenuRaw, statusMenuEle, totalElementalResistance, rawFlatBonusDmg, eleFlatBonusDmg;
        static double raw, ele, totalRaw, totalEle, totalRawCrit, totalEleCrit, totalDmg, totalDmgCrit;
        static string weaponType, weaponTypeBuff, swapScrollColor;
        static float rawSharpness, eleSharpness, affinity, motionValue, rawHitzone, eleHitzone,
                     rawPercentBonusDmg, elePercentBonusDmg, rawDmgMultiplier, eleDmgMultiplier,
                     rawCritPercent, eleCritPercent;

        //Base Game Armor Skill variables
        static byte rapidMorph, offensiveGuard, ammoTypeUp, specialAmmoBoost, attackBoost, bludgeoner,
                    mindsEye, heroics, dragonheart, agitator, peakPerformance, resuscitate, resentment,
                    counterstrike, punishingDraw, coalescence, blightAttack, elderDragonBlessing,
                    criticalElement, criticalEye, weaknessExploit, criticalDraw, maximumMight,
                    latentPower, criticalBoost;
        static bool hasHornMaestro, hasFortify;

        //Sunbreak Armor Skill variables
        static byte sneakAttack, adrenalineRush, foray, buildupBoost, dereliction, bloodAwakening, mailOfHellfire,
                    frostCraftGauge, bloodlust, burst, stormsoul, dragonConversion, strife, elementalExploit;
        static bool hasInspiration, hasAmpedBloodAwakeningEffect, hasAmpedStrifeEffect;

        //Rampage Skill variables
        static bool hasBrutalStrike, hasDullingStrike, hasElementalExploitRampage, hasSilkbindBoost;

        //Buff variables
        static string demondrugType;
        static byte dangoBooster;
        static bool hasPowercharm, hasPowerTalon, hasMightSeed, hasDemonPowder, hasPowerDrum, hasRousingRoar,
                    hasWaterblight;

        //Hunting horn variables
        static bool hasAttackUpSong, hasElementalUpSong, hasAffinityUpSong;
        #endregion

        public static void Main(string[] args)
        {
            string input = "";

            do
            {
                if (input != "replace")
                {
                    //If the player retries their calculations, make sure to set any values
                    //that they may not be asked to their default values (0 or false).
                    rapidMorph = 0;
                    offensiveGuard = 0;
                    ammoTypeUp = 0;
                    specialAmmoBoost = 0;
                    hasHornMaestro = false;
                    hasDullingStrike = false;
                    
                    //Even though these are for Sunbreak armor skills. These are special...
                    //They still go here.
                    hasAmpedBloodAwakeningEffect = false;
                    hasAmpedStrifeEffect = false;

                    do
                    {
                        Write(" Include Sunbreak armor skills? (y/n): ");
                        input = ReadLine().ToLower().Trim();

                        if (input == "yes" || input == "y")
                            includeSunbreakArmorSkills = true;
                        else if (input == "no" || input == "n")
                        {
                            includeSunbreakArmorSkills = false;

                            //In case these had values in a previous calculation that
                            //included Sunbreak armor skills.
                            sneakAttack = 0;
                            adrenalineRush = 0;
                            foray = 0;
                            buildupBoost = 0;
                            dereliction = 0;
                            bloodAwakening = 0;
                            mailOfHellfire = 0;
                            frostCraftGauge = 0;
                            bloodlust = 0;
                            burst = 0;
                            stormsoul = 0;
                            dragonConversion = 0;
                            strife = 0;
                            elementalExploit = 0;
                            hasInspiration = false;
                        }
                            
                    } while (input == "" || input != "yes" && input != "y" &&
                             input != "no" && input != "n");

                    #region Basic Weapon / Player Function Calls
                    WriteLine("\n ### Basic Weapon / Player Info ###");
                    InputWeaponType();
                    InputWeaponTypeBuff();

                    if (includeSunbreakArmorSkills)
                        InputSwapScrollColor();

                    InputRawDamage();
                    InputElementalDamage();

                    if (!weaponType.Contains("bow"))
                        InputSharpness();
                    else
                    {
                        enteredSharpness = "";
                        rawSharpness = 1;
                        eleSharpness = 1;
                    }
                    
                    InputAffinity();
                    InputMotionValue();
                    InputRawHitzone();
                    InputElementalHitzone();

                    WriteLine("\n ### Customizable Numbers ###");
                    InputRawFlatBonusDamage();
                    InputRawPercentBonusDamage();
                    InputElementalFlatBonusDamage();
                    InputElementalPercentBonusDamage();
                    InputRawDamageMultipler();
                    InputElementalDamageMultipler();
                    #endregion

                    #region Base Game Armor Skill Function Calls
                    WriteLine("\n ### Base Game Armor Skills ###");
                    if (weaponType == "hunting horn")
                        InputHornMaestro();

                    if (weaponType == "switch axe" || weaponType == "chargeblade")
                        InputRapidMorph();

                    if (weaponType == "greatsword" || weaponType == "chargeblade" ||
                        weaponType == "sword & shield" || weaponType == "lance" ||
                        weaponType == "gunlance")
                        InputOffensiveGuard();

                    if (weaponType.Contains("bow"))
                    {
                        InputAmmoTypeUp();
                        InputSpecialAmmoBoost();
                    }
                    
                    InputAttackBoost();
                    InputBludgeoner();
                    InputMindsEye();
                    InputHeroics();
                    InputDragonheart();
                    InputAgitator();
                    InputPeakPerformance();
                    InputResuscitate();
                    InputResentment();
                    InputCounterstrike();
                    InputPunishingDraw();
                    InputCoalescence();                                      
                    InputFortify();
                    

                    //Strickly elemental armor skills
                    InputBlightAttack();
                    InputElderDragonBlessing();
                    InputCriticalElement();

                    //Affinity armor skills
                    InputCriticalEye();
                    InputWeaknessExploit();
                    InputCriticalDraw();
                    InputMaximumMight();
                    InputLatentPower();
                    InputCriticalBoost();
                    #endregion

                    #region Sunbreak Armor Skills Function Calls
                    if (includeSunbreakArmorSkills)
                    {
                        ResetColor();
                        WriteLine("\n ### Sunbreak Armor Skills ###");
                        InputSneakAttack();
                        InputBuildupBoost();
                        InputAdrenalineRush();                       
                        InputForay();
                        InputDereliction();
                        InputBloodAwakening();
                        InputMailOfHellfire();
                        InputFrostCraft();
                        InputBloodlust();
                        InputBurst();
                        InputInspiration();
                        InputStormsoul();
                        InputDragonConversion();
                        InputStrife();
                        InputElementalExploit();
                    }                    
                    #endregion

                    #region Rampage Skill Function Calls
                    ResetColor();
                    WriteLine("\n ### Rampage Skills ###");
                    InputBrutalStrike();

                    if (!weaponType.Contains("bow"))
                        InputDullingStrike();

                    InputElementalExploitRampage();
                    InputSilkbindBoost();
                    #endregion

                    #region Buff Function Calls
                    ResetColor();
                    WriteLine("\n ### Buffs ###");
                    InputPowercharm();
                    InputPowertalon();
                    InputMightSeed();
                    InputDemonPowder();
                    InputDemondrugType();
                    InputPalicoPowerDrum();
                    InputRousingRoar();
                    InputDangoBooster();
                    InputWaterblight();
                    #endregion

                }

                CalculatePhase1Damage();
                CalculatePhase2Damage();
                CalculatePhase3TotalDamage();
                DisplayTotalDamage();
               
                //Interface questions for redoing calculations, replacing values, or exiting the program.
                ResetColor();
                do
                {
                    Write(" Enter (redo / replace / exit): ");
                    input = ReadLine().ToLower().Trim();
                } while (input == "" || input != "redo" && input != "replace" && input != "exit");

                if (input == "exit")
                    Environment.Exit(0);
                else if (input == "replace")
                {
                    do
                    {
                        Write(" Enter the name of a value to replace: ");
                        input = ReadLine().ToLower().Trim();
                    } while (!ReplaceValue(input) || input == "back");
                    input = "replace";
                }

                Clear();
            } while (input != "exit");

            ReadKey();
        }

        #region Basic Weapon / Player Input
        static void InputWeaponType()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Weapon Type: "); ;
                input = ReadLine().ToLower().Trim();

                switch (input)
                {
                    case "?":
                        WriteLine("\n 1.  Greatsword         2.  Longsword\n" +
                                  " 3.  Sword & Shield     4.  Dual Blades\n" +
                                  " 5.  Lance              6.  Gunlance\n" +
                                  " 7.  Hammer             8.  Hunting Horn\n" +
                                  " 9.  Switch Axe         10. Chargeblade\n" +
                                  " 11. Insect Glaive      12. Light Bowgun\n" +
                                  " 13. Heavy Bowgun       14. Bow\n\n" +
                                  " The weapon type cannot be changed once a value is entered.\n");
                        break;

                    //Input fixing.
                    case "great sword":
                        input = "greatsword";
                        break;
                    case "long sword":
                        input = "longsword";
                        break;
                    case "swordshield":
                    case "sword and shield":
                    case "sword shield":
                        input = "sword & shield";
                        break;
                    case "dual blade":
                        input = "dual blades";
                        break;
                    case "gun lance":
                        input = "gunlance";
                        break;
                    case "switchaxe":
                        input = "switch axe";
                        break;
                    case "charge blade":
                        input = "chargeblade";
                        break;
                    case "light bow gun":
                        input = "light bowgun";
                        break;
                    case "heavy bow gun":
                        input = "heavy bowgun";
                        break;
                }

            } while (input == "" || input != "greatsword" && input != "longsword" && input != "sword & shield" &&
                     input != "dual blades" && input != "lance" && input != "gunlance" && input != "hammer" &&
                     input != "hunting horn" && input != "switch axe" && input != "chargeblade" && 
                     input != "insect glaive" && input != "light bowgun" && input != "heavy bowgun" && input != "bow");
            weaponType = input;
        }

        static void InputWeaponTypeBuff() 
        {
            string input;
            weaponTypeBuff = ""; //If the user has a weapon with no weapon specific buff.

            if (weaponType == "greatsword")
            {
                do
                {
                    ResetColor();
                    Write(" Power Sheathe (y/n): ");
                    input = ReadLine().ToLower().Trim();

                    if (input == "?")
                    {
                        WriteLine("\n Power Sheathe: Greatsword silkbind switch skill that increases raw damage \n" +
                                  "                by +10% for 20 seconds.\n");
                    }
                    else if (input == "back")
                        WriteLine("\n The weapon type cannot be changed once entered...\n");
                } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");

                if (input == "yes" || input == "y")
                    weaponTypeBuff = "power sheathe";
            }
            else if (weaponType == "longsword")
            {
                WriteLine("\n Spirit Gauge:\n" +
                          " 0. No Spirit Gauge\n" +
                          " 1. White Spirit Gauge\n" +
                          " 2. Yellow Spirit Gauge\n" +
                          " 3. Red Spirit Gauge\n\n");

                do
                {
                    ResetColor();
                    Write(" Spirit Gauge Level (0-3): ");
                    input = ReadLine().ToLower().Trim();

                    if (input == "back")
                    {
                        WriteLine("\n Spirit Gauge Levels: A sword shaped meter that fills as the player attacks a monster.\n" +
                                  "                      Using the Spirit Blade combo expends this, but if the last hit of\n" +
                                  "                      the combo strikes a monster, then the gauge will change color (up\n" +
                                  "                      to 3 times). The color will also change if the player counters a\n" +
                                  "                      monster's attack with Iai Spirit Slash or if they land a hit with" +
                                  "                      the Sakura Slash attack\n\n" +
                                  "                      White Gauge:  +4% raw and elemental damage.\n" +
                                  "                      Yellow Gauge: +8% raw and elemental damage.\n" +
                                  "                      Red:          +12% raw and elemental damage.\n");
                    }
                    else if (input == "back")
                        WriteLine("\n The weapon type cannot be changed once entered...\n");
                } while (input == "" || input != "0" && input != "1" && input != "2" && input != "3");

                if (input != "0")
                    weaponTypeBuff = "spirit gauge level " + input;
            }
            else if (weaponType == "dual blades")
            {
                WriteLine("\n Dual blades modes:\n" +
                          " 0. Default mode\n" +
                          " 1. Demon mode\n" +
                          " 2. Feral Demon Mode\n" +
                          " 3. Archdemon Mode\n\n");

                do
                {
                    ResetColor();
                    Write(" Dual Blades Mode (0-3): ");
                    input = ReadLine().ToLower().Trim();

                    if (input == "?")
                    {
                        WriteLine("\n Dual Blades Modes: The player may enter Demon or Feral Demon Mode at any time. Attacking\n" +
                                  "                    in these modes will fill a sword shaped meter. If the player exits\n" +
                                  "                    these modes with the meter filled, they will enter Archdemon Mode.\n" +
                                  "                    Attacking then will expend the meter.\n\n" +
                                  "                    Demon Mode:       +35% elemental damage.\n" +
                                  "                    Feral Demon Mode: +20% raw damage.\n" +
                                  "                    Archdemon Mode:   +20% elemental damage.\n");
                    }
                    else if (input == "back")
                        WriteLine("\n The weapon type cannot be changed once entered...\n");
                } while (input == "" || input != "0" && input != "1" && input != "2" && input != "3");

                switch (input)
                {
                    case "1":
                        weaponTypeBuff = "demon mode";
                        break;
                    case "2":
                        weaponTypeBuff = "feral demon mode";
                            break;
                    case "3":
                        weaponTypeBuff = "archdemon mode";
                        break;
                }
            }
            else if (weaponType == "hunting horn")
            {
                do
                {
                    ResetColor();
                    Write(" Infernal Melody (y/n): ");
                    input = ReadLine().ToLower().Trim();
                    
                    if (input == "?")
                    {
                        WriteLine("\n Infernal Melody: Attacking fills a circle meter. Once the meter is filled, the player\n" +
                            "                        may perform the Infernal Melody song at the end of the Magnificent Trio\n" +
                            "                        attack. Infernal Melody increases raw damage by +20%, but replaces all\n" +
                            "                        other Attack Up song effects. The song lasts 20 seconds, after which if\n" +
                            "                        another Attack Up song is still active, the player will fall back to that.\n");
                    }
                    else if (input == "back")
                        WriteLine("\n The weapon type cannot be changed once entered...\n");
                } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");

                if (input == "yes" || input == "y")
                    weaponTypeBuff = "infernal melody";
            }
            else if (weaponType == "chargeblade")
            {
                do
                {
                    ResetColor();
                    Write(" Charged Shield (y/n): ");
                    input = ReadLine().ToLower().Trim();

                    if (input == "?")
                    {
                        WriteLine("\n Charged Shield: If the player has any phials, they may perform the Elemental Roundslash\n" +
                                  "                 attack to charge the shield. While in Axe Mode, the shield will increase\n" +
                                  "                 raw damage by +10%. This damage increase will not appear in the Status\n" +
                                  "                 Menu though.\n");
                    }
                    else if (input == "back")
                        WriteLine("\n The weapon type cannot be changed once entered...\n");
                } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");

                if (input == "yes" || input == "y")
                    weaponTypeBuff = "charged shield";
            }
            else if (weaponType == "switch axe")
            {
                do
                {
                    ResetColor();
                    Write(" Using Sword Mode (y/n): ");
                    input = ReadLine().ToLower().Trim();

                    if (input == "back")
                        WriteLine("\n The weapon type cannot be changed once entered...\n");
                } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");

                if (input == "yes" || input == "y")
                {
                    WriteLine("\n Switch Axe phial types:\n" +
                              " 1. Power Phial\n" +
                              " 2. Elemental Phial\n\n");

                    do
                    {                       
                        Write(" Phial Type (1-2): ");
                        input = ReadLine().ToLower().Trim();
                         
                        if (input == "?")
                        {
                            WriteLine("\n Phial Type: When the player is using Sword Mode, their phial type will increase\n" +
                                      "             damage but this will not appear in the Status Menu.\n\n" +
                                      "             Power Phial:     +15% raw damage.\n" +
                                      "             Elemental Phial: +45% elemental damage.\n");
                        }
                        else if (input == "back")
                            WriteLine("\n The weapon type cannot be changed once entered...\n");
                    } while (input == "" || input != "1" && input != "2");

                    if (input == "1")
                        weaponTypeBuff = "power phial";
                    else
                        weaponTypeBuff = "elemental phial";
                }
            }
            else if (weaponType == "insect glaive")
            {
                WriteLine("\n Kinsects active:\n" +
                          " 0. No combinations that increase damage\n" +
                          " 1. Red and white kinsects\n" +
                          " 2. Red, white, and orange kinsects\n\n");
                
                do
                {
                    ResetColor();
                    Write(" Kinsects active (0-2): ");
                    input = ReadLine().ToLower().Trim();

                    if (input == "?")
                    {
                        WriteLine("\n Kinsects: The player may perform the Harvest Extract and Recall attacks to gather\n" +
                                  "           kinsects from monster parts that give them several buffs.\n\n" +
                                  "           Red:          Increased moveset.\n" +
                                  "           Orange:       Increased defense.\n" +
                                  "           White:        Increased movement speed.\n" +
                                  "           Red + White:  +10% raw damage.\n" +
                                  "           All Kinsects: +20% raw damage.\n");
                    }
                    else if (input == "back")
                        WriteLine("\n The weapon type cannot be changed once entered...\n");
                } while (input == "" || input != "0" && input != "1" && input != "2");

                if (input == "1")
                    weaponTypeBuff = "red and white kinsects active";
                else if (input == "2")
                    weaponTypeBuff = "all kinsects active";
            }
        }

        static void InputSwapScrollColor()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Swap Scroll Color (Red / Blue): ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Swap Scroll Color: In the Sunbreak DLC, the Swap Scroll allows the player to change\n" +
                              "                    between switch skill loadouts. There are also several armor skills\n" +
                              "                    that will also be affected by the swap scroll color.\n");
                }
                else if (input == "back")
                    ReplaceValue("weapon type buff");
            } while (input == "" || input != "red" && input != "blue");
            swapScrollColor = input;
        }

        static void InputRawDamage()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Raw Damage: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                    WriteLine("\n Raw Damage: The default non-elemental damage found on the weapon.\n");
                else if (input == "back" && includeSunbreakArmorSkills)
                    ReplaceValue("swap scroll color");
                else if (input == "back")
                    ReplaceValue("weapon type buff");

            } while (!int.TryParse(input, out enteredRaw) || enteredRaw < 0);
        }

        static void InputElementalDamage()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Elemental Damage: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Elemental Damage: The default elemental damage of the weapon.\n\n" +
                             " Blights:       Statuses:\n" +
                             " 1. Fire        1. Sleep\n" +
                             " 2. Water       2. Blast\n" +
                             " 3. Ice         3. Paralysis\n" +
                             " 4. Thunder     4. Poison\n" +
                             " 5. Dragon\n\n" +
                             " Use this only for elemental blights. Elemental status damage\n" +
                             " does not deal direct damage to a monster's health bar. Instead it\n" +
                             " will deal damage to its immunity. Once it reaches 0, that status's\n" +
                             " affect will activate (and might deal damage but this calculator is\n" +
                             " not for that)\n");
                }
                else if (input == "back")
                    ReplaceValue("raw damage");

            } while (!int.TryParse(input, out enteredEle) || enteredEle < 0);
        }

        static void InputSharpness()
        {
            do
            {
                ResetColor();
                Write(" Sharpness: ");
                enteredSharpness = ReadLine().ToLower().Trim();

                if (enteredSharpness == "?")
                {
                    WriteLine("\n Sharpness: Each color has a seperate multiplier for raw and elemental damage.\n\n" +
                              "          Raw:     Elemental:\n" +
                              " Red:     0.50     0.25\n" +
                              " Orange:  0.75     0.50\n" +
                              " Yellow:  1.00     0.75\n" +
                              " Green:   1.05     1.00\n" +
                              " Blue:    1.20     1.0625\n" +
                              " White:   1.32     1.15\n" +
                              " Purple:  1.39     1.25\n");
                }
                else if (enteredSharpness == "back")
                    ReplaceValue("elemental damage");

            } while (enteredSharpness == "" || enteredSharpness != "red" && enteredSharpness != "orange" &&
                     enteredSharpness != "yellow" && enteredSharpness != "green" && enteredSharpness != "blue" &&
                     enteredSharpness != "white" && enteredSharpness != "purple");

            //Assigning numerical values to raw and elemental sharpness variables.
            switch (enteredSharpness) 
            {
                case "red":
                    rawSharpness = 0.50f;
                    eleSharpness = 0.25f;
                    break;
                case "orange":
                    rawSharpness = 0.75f;
                    eleSharpness = 0.50f;
                    break;
                case "yellow":
                    rawSharpness = 1.00f;
                    eleSharpness = 0.75f;
                    break;
                case "green":
                    rawSharpness = 1.05f;
                    eleSharpness = 1.00f;
                    break;
                case "blue":
                    rawSharpness = 1.20f;
                    eleSharpness = 1.0625f;
                    break;
                case "white":
                    rawSharpness = 1.32f;
                    eleSharpness = 1.15f;
                    break;
                case "purple":
                    rawSharpness = 1.39f;
                    eleSharpness = 1.25f;
                    break;
            }
        }

        static void InputAffinity()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Affinity: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Affinity: The chance of landing a critical hit. By default, a critical hit\n" +
                              "           deals +25% raw damage.\n");
                }
                else if (input == "back" && weaponType.Contains("bow"))
                    ReplaceValue("elemental damage");
                else if (input == "back")
                    ReplaceValue("sharpness");

            } while (!float.TryParse(input, out enteredAffinity) || enteredAffinity % 1 != 0 ||
                     enteredAffinity < -100 || enteredAffinity > 100);
            enteredAffinity /= 100;
        }

        static void InputMotionValue()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Motion Value: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Motion Value: A number given to each attack to distinguish them.\n" +
                              "               The higher the motion value, the more powerful the attack.\n");
                }
                else if (input == "back")
                    ReplaceValue("affinity");

            } while (!float.TryParse(input, out motionValue) || motionValue % 1 != 0 || motionValue < 0);
            motionValue /= 100;
        }

        static void InputRawHitzone()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Raw Hitzone: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Raw Hitzone: Each monster part is given several numbers to distinguish how\n" +
                              "              weak they are to certain types of damage. The higher the hitzone,\n" +
                              "              the weaker the part is. For raw damage, a part will have raw\n" +
                              "              hitzones for sever, blunt, and shot attacks. For elemental damage,\n" +
                              "              a part will have elemental hitzones for each elemental blight.\n");
                }
                else if (input == "back")
                    ReplaceValue("motion value");
               

            } while (!float.TryParse(input, out enteredRawHitzone) || enteredRawHitzone % 1 != 0 ||
                     enteredRawHitzone < 0);
            enteredRawHitzone /= 100;
        }

        static void InputElementalHitzone()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Elemental Hitzone: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Elmental Hitzone: Each monster part is given several numbers to distinguish how\n" +
                              "                   weak they are to certain types of damage. The higher the hitzone,\n" +
                              "                   the weaker the part is. For raw damage, a part will have raw\n" +
                              "                   hitzones for sever, blunt, and shot attacks. For elemental damage,\n" +
                              "                   a part will have elemental hitzones for each elemental blight.\n");
                }
                else if (input == "back")
                    ReplaceValue("raw hitzone");

            } while (!float.TryParse(input, out enteredEleHitzone) || enteredEleHitzone % 1 != 0 ||
                     enteredEleHitzone < 0);
            enteredEleHitzone /= 100;
        }

        static void InputRawFlatBonusDamage()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Raw flat bonus damage: ");
                input = ReadLine().Trim().ToLower();

                if (input == "?")
                {
                    WriteLine("\n Raw Flat Damage Bonus: May be used to increase raw damage by a flat rate. This increase\n" +
                              "                        can be seen in the Status Menu. Raw damage from a player's Petalace\n" +
                              "                        may be added here. The number will be labled Attack Up in the\n" +
                              "                        Equipment Menu.\n");
                }
                else if (input == "back")
                    ReplaceValue("elemental hitzone");

            } while (!int.TryParse(input, out enteredRawFlatBonusDmg) || enteredRawFlatBonusDmg < 0);
        }

        static void InputRawPercentBonusDamage()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Raw percent bonus damage: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Raw Percent Bonus Damage: May be used to increase raw damage by a percentage. This\n" +
                              "                           increase may be seen in the Status Menu.\n");
                }
                else if (input == "back")
                    ReplaceValue("raw flat bonus damage");

            } while (!float.TryParse(input, out rawPercentBonusDmg) || rawPercentBonusDmg % 1 != 0 ||
                     rawPercentBonusDmg < 0);
            rawPercentBonusDmg = 1 + (rawPercentBonusDmg / 100);
        }

        static void InputElementalFlatBonusDamage()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Elemental flat bonus damage: ");
                input = ReadLine().Trim().ToLower();

                if (input == "?")
                {
                    WriteLine("\n Elemental Flat Bonus Damage: May be used to increase elemental damage by a flat rate.\n" +
                              "                              This increase may be seen in the Status Menu.\n");
                }
                else if (input == "back")
                    ReplaceValue("raw percent bonus damage");

            } while (!int.TryParse(input, out enteredEleFlatBonusDmg) || enteredRawFlatBonusDmg < 0);
        }
        static void InputElementalPercentBonusDamage()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Elemental percent bonus damage: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Elemental Percent Bonus Damage: May be used to increase elemental damage by a percentage.\n" +
                              "                                 This increase may be seen in the Status Menu.\n");
                }
                else if (input == "back")
                    ReplaceValue("elemental flat bonus damage");

            } while (!float.TryParse(input, out elePercentBonusDmg) || elePercentBonusDmg % 1 != 0 ||
                     elePercentBonusDmg < 0);
            elePercentBonusDmg = 1 + (elePercentBonusDmg / 100);
        }

        static void InputRawDamageMultipler()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Raw damage multiplier: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Raw Damage Multiplier: May be used to increase raw damage by a percentage after the\n" +
                              "                        Status Menu damage has been calculated.\n");
                }
                else if (input == "back")
                    ReplaceValue("elemental percent bonus damage");

            } while (!float.TryParse(input, out rawDmgMultiplier) || rawDmgMultiplier % 1 != 0 ||
                     rawDmgMultiplier < 0);
            rawDmgMultiplier = 1 + (rawDmgMultiplier / 100);
        }

        static void InputElementalDamageMultipler()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Elemental damage multiplier: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    WriteLine("\n Elemental Damage Multiplier: May be used to increase elemental damage by a percentage\n" +
                              "                              after the Status Menu damage has been calculated.\n");
                }
                else if (input == "back")
                    ReplaceValue("raw damage multiplier");

            } while (!float.TryParse(input, out eleDmgMultiplier) || eleDmgMultiplier % 1 != 0 ||
                     eleDmgMultiplier < 0);
            eleDmgMultiplier = 1 + (eleDmgMultiplier / 100);
        }
        #endregion

        #region Base Game Armor Skill Input
        static void InputHornMaestro()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Horn Maestro (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasHornMaestro = true;
                else if (input == "no" || input == "n")
                    hasHornMaestro = false;
                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Horn Maestro: Increases the raw damage for sonic attacks by +10%. This damage\n" +
                              "               increase will not appear in the Status Menu. Remember that sonic\n" +
                              "               attacks ignore hitzone values, do not deal elemental damage, and\n" +
                              "               cannot crit.\n");
                }
                else if (input == "back")
                    ReplaceValue("elemental damage multiplier");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputRapidMorph()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Rapid Morph: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Rapid Morph: Increases the speed and raw damage of morph attacks. This damage\n" +
                              "              increase will not appear in the Status Menu.\n" +
                              " Lvl 1: Just increases switch speed.\n" +
                              " Lvl 2: +10% raw damage.\n" +
                              " Lvl 3: +20% raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("elemental damage multiplier");
            } while (!byte.TryParse(input, out rapidMorph) || rapidMorph < 0 || rapidMorph > 3);
        }

        static void InputOffensiveGuard()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Offensive Guard: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Offensive Guard: Increases raw damage after the player performs a perfectly timed guard.\n" +
                              " Lvl 1: +5% raw damage.\n" +
                              " Lvl 2: +10% raw damage.\n" +
                              " Lvl 3: +15% raw damage.\n");
                }
                else if (input == "back" && (weaponType == "chargeblade" || weaponType == "switch axe"))
                    ReplaceValue("rapid morph");
                else if (input == "back")
                    ReplaceValue("elemental damage multiplier");
            } while (!byte.TryParse(input, out offensiveGuard) || offensiveGuard < 0 || offensiveGuard > 3);
        }

        static void InputAmmoTypeUp()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Ammo Type Up: ");
                input = ReadLine().ToLower().Trim();
                
                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Ammo Type Up: This is a catch all armor skill for the Pierce Up, Spread Up, Rapid Fire Up, and\n" +
                              "               Normal/Rapid Up armor skills. These each increase raw damage when the player is using\n" +
                              "               a certain type of ammo or arrows. This damage increase will not appear in the Status Menu." +
                              " Lvl 1: +5% raw damage.\n" +
                              " Lvl 2: +10% raw damage.\n" +                      
                              " Lvl 3: +20% raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("elemental damage multiplier");

            } while (!byte.TryParse(input, out ammoTypeUp) || ammoTypeUp < 0 || ammoTypeUp > 3);
        }

        static void InputSpecialAmmoBoost()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Special Ammo Boost: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine(" Special Ammo Boost: Increases raw damage when the player uses Bowgun special ammo or\n" +
                              "                     Bow Dragon Piercer. This damage increase will not appear in\n" +
                              "                     the Status Menu\n" +
                              " Lvl 1: +10% raw damage.\n" +
                              " Lvl 2: +20% raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("ammo type up");

            } while (!byte.TryParse(input, out specialAmmoBoost) || specialAmmoBoost < 0 ||
                     specialAmmoBoost > 2);
        }

        static void InputAttackBoost()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Attack Boost: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Attack Boost: Increases raw damage.\n" +
                              " Lvl 1: +3 raw damage.\n" +
                              " Lvl 2: +6 raw damage.\n" +
                              " Lvl 3: +9 raw damage.\n" +
                              " Lvl 4: +5% raw damage and +7 raw bonus damage.\n" +
                              " Lvl 5: +6% raw damage and +8 raw bonus damage.\n" +
                              " Lvl 6: +8% raw damage and t+7 raw bonus damage.\n" +
                              " Lvl 7: +10% raw damage and +10 raw bonus damage.\n");
                }
                else if (input == "back" && weaponType == "hunting horn")
                    ReplaceValue("horn maestro");
                else if (input == "back" && (weaponType == "greatsword" || weaponType == "chargeblade" ||
                         weaponType == "sword & shield" || weaponType == "lance" || weaponType == "gunlance"))
                    ReplaceValue("offensive guard");
                else if (input == "back" && weaponType == "switch axe")
                    ReplaceValue("rapid morph");
                else if (input == "back" && weaponType.Contains("bow"))
                    ReplaceValue("special ammo boost");
                else if (input == "back")
                    ReplaceValue("elemental damage multiplier");

            } while (!byte.TryParse(input, out attackBoost) || attackBoost < 0 || attackBoost > 7);
        }

        static void InputBludgeoner()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Bludgeoner: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Blugeoner: Increases raw damage depending on the player's sharpness.\n" +
                              " Lvl 1: +5% raw damage when sharpness is yellow or lower\n" +
                              " Lvl 2: +10% raw damage when sharpness is yellow or lower\n" +
                              " Lvl 3: +10% raw damage when sharpness is green or lower\n");
                }
                else if (input == "back")
                    ReplaceValue("attack boost");
            } while (!byte.TryParse(input, out bludgeoner) || bludgeoner < 0 || bludgeoner > 3);
        }

        static void InputMindsEye()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Mind's Eye: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Mind's Eye: Increases raw damage when the player strikes hard monster parts. A monster part\n" +
                              "             is considered hard when the product of its raw hitzone and the player's raw sharpness\n" +
                              "             is less than 45%. This damage increase will not appear in the Status Menu. This armor\n" +
                              "             skill will also prevent attacks from deflecting.\n" +
                              " Lvl l: +10% raw damage.\n" +
                              " Lvl 2: +15% raw damage.\n" +
                              " Lvl 3: +30% raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("bludgeoner");
            } while (!byte.TryParse(input, out mindsEye) || mindsEye < 0 || mindsEye > 3);
        }

        static void InputHeroics()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Heroics: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Heroics: Increases raw damage and defense when the player's health drops to 35% or lower.\n" +
                              " Lvl 1:   Just increases defense.\n" +
                              " Lvl 2-3: +5% raw damage.   \n" +
                              " Lvl 4:   +10% raw damage.\n" +
                              " Lvl 5:   +30% raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("mind's eye");
            } while (!byte.TryParse(input, out heroics) || heroics < 0 || heroics > 5);
        }

        static void InputDragonheart()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Dragonheart: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Dragonheart: After losing a certain amount of health, the player contracts Dragonblight, but\n" +
                              "              their elemental resistances and raw damage increase. Note that dragonblight prevents\n" +
                              "              the player from dealing non-dragon elemental blight damage.\n" +
                              " Lvl 1-3: Just raises elemental resistance.\n" +
                              " Lvl 4:   +5% raw damage. Activates when at 70% or less health.\n" +
                              " Lvl 5:   +10% raw damage. Actiavtes when at 80% or less health.\n");
                }
                else if (input == "back")
                    ReplaceValue("heroics");
            } while (!byte.TryParse(input, out dragonheart) || dragonheart < 0 || dragonheart > 5);
        }

        static void InputAgitator()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Agitator: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Agitator: Increases raw damage and affinity when a large monster becomes enraged.\n" +
                              "           The effect lasts until the monster goes out of their enraged state.\n" +
                              " Lvl 1: +4 raw damage and +3% affinity.\n" +
                              " Lvl 2: +8 raw damage and +5% affinity.\n" +
                              " Lvl 3: +12 raw damage and +7% affinity.\n" +
                              " Lvl 4: +16 raw damage and +10% affinity.\n" +
                              " Lvl 5: +20 raw damage and +15% affinity.\n");
                }
                else if (input == "back")
                    ReplaceValue("dragonheart");
            } while (!byte.TryParse(input, out agitator) || agitator < 0 || agitator > 5);
        }

        static void InputPeakPerformance()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Peak Performance: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Peak Performance: Increases raw damage while the player is at full health.\n" +
                              " Lvl 1: +5 raw damage.\n" +
                              " Lvl 2: +10 raw damage.\n" +
                              " Lvl 3: +20 raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("agitator");
            } while (!byte.TryParse(input, out peakPerformance) || peakPerformance < 0 ||
                     peakPerformance > 3);
        }

        static void InputResuscitate()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Resuscitate: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Resuscitate: Increases raw damage while the player is suffering from an abnormal status.\n" +
                              " Lvl 1: +5 raw damage.\n" +
                              " Lvl 2: +10 raw damage.\n" +
                              " Lvl 3: +20 raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("peak performance");
            } while (!byte.TryParse(input, out resuscitate) || resuscitate < 0 || resuscitate > 3);
        }

        static void InputResentment()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Resentment: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Resentment: Increases raw damage when the player has recoverable health (red health).\n" +
                              " Lvl 1: +5 raw damage.\n" +
                              " Lvl 2: +10 raw damage.\n" +
                              " Lvl 3: +15 raw damage.\n" +
                              " Lvl 4: +20 raw damage.\n" +
                              " Lvl 5: +25 raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("resuscitate");
            } while (!byte.TryParse(input, out resentment) || resentment < 0 || resentment > 5);
        }

        static void InputCounterstrike()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Counterstrike: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Counterstrike: Increases raw damage after the player gets knocked down.\n" +
                              "                This effect lasts 30 seconds.\n" +
                              " Lvl 1: +10 raw damage.\n" +
                              " Lvl 2: +15 raw damage.\n" +
                              " Lvl 3: +25 raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("resentment");
            } while (!byte.TryParse(input, out counterstrike) || counterstrike < 0 || counterstrike > 3);
        }

        static void InputPunishingDraw()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Punishing Draw: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Punishing Draw: Increases raw damage when the player performs a draw attack.\n" +
                              "                 This damage will not appear in the Status Menu.\n" +
                              " Lvl 1: +3 raw damage.\n" +
                              " Lvl 2: +5 raw damage.\n" +
                              " Lvl 7: +7 raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("counterstrike");
            } while (!byte.TryParse(input, out punishingDraw) || punishingDraw < 0 || punishingDraw > 3);
        }

        static void InputCoalescence()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Coalescence: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Coalescence: Increases raw and elemental damage after recovering from a blight\n" +
                              "              or abnormal status. This effect lasts 30 seconds.\n" +
                              " Lvl 1: +12 raw damage and +2 elemental damage.\n" +
                              " Lvl 2: +15 raw damage and +3 elemental damage.\n" +
                              " Lvl 3: +18 raw damage and +4 elemental damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("punishing draw");
            } while (!byte.TryParse(input, out coalescence) || coalescence < 0 || coalescence > 3);
        }

        static void InputFortify()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Fortify (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasFortify = true;
                else if (input == "no" || input == "n")
                    hasFortify = false;
                else if (input == "?") {
                    ResetColor();
                    WriteLine("\n Fortify: Increases raw damage by +10% every time the player faints. For maxmimum damage,\n" +
                              "          the calculator will assume that the player has already fainted twice.\n");
                }
                else if (input == "back")
                    ReplaceValue("coalescence");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputBlightAttack()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Cyan;
                Write(" Elemental Blight Attack: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Elemental Blight Attack: This is a catch all armor skill for Fire, Water, Ice, Thunder,\n" +
                              "                          and Drgaon Attack Up.\n" +
                              " Lvl 1: +2 elemental damage\n" +
                              " Lvl 2: +3 elemental damage.\n" +
                              " Lvl 3: +5% elemental damage and +4 elemental bonus damage.\n" +
                              " Lvl 4: +10% elemental damage and +4 elemental bonus damage.\n" +
                              " Lvl 5: +20% elemental damage and +4 elemental bonus damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("fortify");
            } while (!byte.TryParse(input, out blightAttack) || blightAttack < 0 || blightAttack > 5);
        }

        static void InputElderDragonBlessing()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Cyan;
                Write(" Kushala's or Teostra's Blessing: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Kushala's Blessing: Increases water and ice elemental damage (and adds other effects). The\n" +
                              "                     calculator will assume the player is using the correct elements.\n" +
                              " Lvl 1: +5% water and ice elemental damage.\n" +
                              " Lvl 2: +10% water and ice elemental damage.\n" +
                              " Lvl 3: Allows recovery to exceed teh red portion of the health gauge.\n" +
                              " Lvl 4: Nullifies heat damage.\n\n" +
                              " Teostra's Blessing: Increases fire elemental damage and blast buildup (and adds other effects).\n" +
                              "                     The calculator will assume the player is using the correct element.\n" +
                              " Lvl 1: +5% fire elemental damage.\n" +
                              " Lvl 2: +10% fire elemental damage.\n" +
                              " Lvl 3: Prevents poison and venom.\n" +
                              " Lvl 4: Prevents theft.\n");
                }
                else if (input == "back")
                    ReplaceValue("blight attack");
            } while (!byte.TryParse(input, out elderDragonBlessing) || elderDragonBlessing < 0 ||
                     elderDragonBlessing > 5);
        }

        static void InputCriticalElement()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Cyan;
                Write(" Critical Element: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Critical Element: Increases elemental damage when the player lands a critical hit.\n" +
                              "                   Negative affinity and Critical Boost do not affect this.\n" +
                              " Lvl 1: Critical hits deal +5% elemental damage.\n" +
                              " Lvl 2: Critical hits deal +10% elemental damage.\n" +
                              " Lvl 3: Critical hits deal +15% elemental damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("elder dragon blessing");
            } while (!byte.TryParse(input, out criticalElement) || criticalElement < 0 || criticalElement > 3);
        }

        static void InputCriticalEye()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Magenta;
                Write(" Critical Eye: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Critical Eye: Increases affinity.\n" +
                              " Lvl 1: +5% affinity.\n" +
                              " Lvl 2: +10% affinity.\n" +
                              " Lvl 3: +15% affinity.\n" +
                              " Lvl 4: +20% affinity.\n" +
                              " Lvl 5: +25% affinity.\n" +
                              " Lvl 6: +30% affinity.\n" +
                              " Lvl 7: +40% affinity.\n");
                }
                else if (input == "back")
                    ReplaceValue("critical element");
            } while (!byte.TryParse(input, out criticalEye) || criticalEye < 0 || criticalEye > 7);
        }

        static void InputWeaknessExploit()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Magenta;
                Write(" Weakness Exploit: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Weakness Exploit: Increases affinity when the player attacks a weak monster part.\n" +
                              "                   A weak monter part is one with a raw hitzone of at least 45%\n" +
                              "                   or higher. This damage increase will not appear in the Status Menu.\n" +
                              " Lvl 1: +15% affinity.\n" +
                              " Lvl 2: +30% affinity.\n" +
                              " Lvl 3: +50% affinity.\n");
                }
                else if (input == "back")
                    ReplaceValue("critical eye");               
            } while (!byte.TryParse(input, out weaknessExploit) || weaknessExploit < 0 ||
                     weaknessExploit > 3);
        }

        static void InputCriticalDraw()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Magenta;
                Write(" Critical Draw: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Critical Draw: Increases affinity for draw attacks, not including silkbind attacks.\n" +
                              "                This will not appear in the Status Menu.\n" +
                              " Lvl 1: +15% affinity.\n" +
                              " Lvl 2: +30% affinity.\n" +
                              " Lvl 3: +60% affininty.\n");
                }
                else if (input == "back")
                    ReplaceValue("weakness exploit");
            } while (!byte.TryParse(input, out criticalDraw) || criticalDraw < 0 || criticalDraw > 3);
        }

        static void InputMaximumMight()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Magenta;
                Write(" Maximum Might: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Maximum Might: Increases affinity after stamina is kept full for a period of time.\n" +
                              "                This activates after 3 seconds of full stamina and deactivates after\n" +
                              "                the player spends 2 seconds without a full bar.\n" +
                              " Lvl 1: +10% affinity.\n" +
                              " Lvl 2: +20% affinity.\n" +
                              " Lvl 3: +30% affinity.\n");
                }
                else if (input == "back")
                    ReplaceValue("critical draw");
            } while (!byte.TryParse(input, out maximumMight) || maximumMight < 0 || maximumMight > 3);
        }

        static void InputLatentPower()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Magenta;
                Write(" Latent Power: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Latent Power: Increases affinity and reduces stamina depletion once the player spends\n" +
                              "               at least 2 minutes and 45 seconds fighting a large monster. Any time the\n" +
                              "               player spends in an attack animation will speed this timer up.\n" +
                              " Lvl 1: +10% affinity.\n" +
                              " Lvl 2: +20% affinity.\n" +
                              " Lvl 3: +30% affinity.\n" +
                              " Lvl 4: +40% affinity.\n" +
                              " Lvl 5: +50% affinity.\n");
                }
                else if (input == "back")
                    ReplaceValue("maximum might");
            } while (!byte.TryParse(input, out latentPower) || latentPower < 0 || latentPower > 5);
        }

        static void InputCriticalBoost()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Magenta;
                Write(" Critical Boost: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")                   
                {
                    ResetColor();
                    WriteLine("\n Critical Boost: Increases raw damage when the player lands a critical hit. This will\n" +
                              "                 not work when paired with negative affinity and will not increase\n" +
                              "                 elemental damage when paired with Critical Element.\n" +
                              " Lvl 1: Critical hits now deal +30% raw damage.\n" +
                              " Lvl 2: Critical hits now deal +35% raw damage.\n" +
                              " Lvl 3: Critical hits now deal +40% raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("latent power");
            } while (!byte.TryParse(input, out criticalBoost) || criticalBoost < 0 || criticalBoost > 3);
        }
        #endregion

        #region Sunbreak Armor Skill Input
        static void InputSneakAttack()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Sneak Attack: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Sneak Attack: Increases raw damage while the player is attacking a monster from behind.\n" +
                              "               This damage increase will not appear in the Status Menu.\n" +
                              " Lvl 1: +5% raw damage.\n" +
                              " Lvl 2: +10% raw damage.\n" +
                              " Lvl 3: +20% raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("critical boost");
            } while (!byte.TryParse(input, out sneakAttack) || sneakAttack < 0 || sneakAttack > 3);
        }

        static void InputBuildupBoost()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Buildup Boost: "); 
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Buildup Boost: Increases raw damage while the player lands attacks that build up poison,\n" +
                              "                paralysis, sleep, blast, or exhaust. Attacks that apply statuses can be\n" +
                              "                seen emitting a colorful cloud. This damage increase will not appear in\n" +
                              "                the Status Menu.\n" +
                              " Lvl 1: +10% raw damage.\n" +
                              " Lvl 2: +15% raw damage.\n" +
                              " LVl 3: +20% raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("sneak attack");
            } while (!byte.TryParse(input, out buildupBoost) || buildupBoost < 0 || buildupBoost > 3);
        }

        static void InputAdrenalineRush()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Adrenaline Rush: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Adrenaline Rush: Increases raw damage after the player has performed a perfectly\n" +
                              "                  timed dodge. This effect lasts 30 seconds.\n" +
                              " Lvl 1: +10 raw damage.\n" +
                              " Lvl 2: +15 raw damage.\n" +
                              " Lvl 3: +30 raw damage.\n");
                }
                else if (input == "back")
                {
                    ReplaceValue("buildup boost");
                }
            } while (!byte.TryParse(input, out adrenalineRush) || adrenalineRush < 0 || adrenalineRush > 3);
        }

        static void InputForay()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Foray: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Foray: Increases raw damage and affinity while the player attacks a large monster affected\n" +
                              "        poison, paralysis, or a blight. This damage increase will not appear in the Status Menu.\n" +
                              " Lvl 1: +10 raw damage.\n" +
                              " Lvl 2: +10 raw damage and +10% affinity.\n" +
                              " Lvl 3: +15% raw damage and +20% affinity.\n");
                }
                else if (input == "back")
                    ReplaceValue("adrenaline rush");
            } while (!byte.TryParse(input, out foray) || foray < 0 || foray > 3);
        }

        static void InputDereliction()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Dereliction: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Dereliction: Increases elemental damage and status buildup while the player is using the\n" +
                              "              Red Swap Scroll. Also increases raw damage and stun damage while they are using\n" +
                              "              the Blue Swap Scroll. This is at the cost of constantly having their health drained\n" +
                              "              by Qurios. But the more Qurios they have, the more damage they will deal.\n" +
                              "              For maxmimum damage, the calculator will assume the player has all 3 Qurios.\n\n" +
                              " Red Scroll:                      Blue Scroll\n" +
                              " Lvl 1: +12 elemental damage.     Lvl 1: +25 raw damage.\n" +
                              " Lvl 2: +15 elemental damage.     Lvl 2: +30 raw damage.\n" +
                              " Lvl 3: +20 elemental damage.     Lvl 3: +35 raw damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("foray");
            } while (!byte.TryParse(input, out dereliction) || dereliction < 0 || dereliction > 3);
        }

        static void InputBloodAwakening()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Blood Awakening: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Blood Awakening: Increaes raw and elemental damage depending on how much health the player gains\n" +
                              "                  through the Bloodrite armor skill or while they are afflicted with Bloodblight.\n" +
                              "                  The armor skill will activate if they gain at least 50 health and will be\n" +
                              "                  amplified if they gain at least 150 health. Both effects last 30 seconds.\n\n" +
                              " 50+ Health Healed:                                 150+ Health Healed:\n" +
                              " Lvl 1: +4 raw and elemental damage.                Lvl 1: +8 raw and elemental damage.\n" +
                              " Lvl 2: +4 raw and elemental damage.                Lvl 2: +15 raw and elemental damage.\n" +
                              " Lvl 3: +10 raw damage and +8 elemental damage.     Lvl 3: +25 raw damage and +15 elemental damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("dereliction");
            } while (!byte.TryParse(input, out bloodAwakening) || bloodAwakening < 0 || bloodAwakening > 3);

            if (bloodAwakening > 0)
                InputAmpedBloodAwakeningEffect();
        }

        static void InputAmpedBloodAwakeningEffect()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Amplified Blood Awakening effect (y/n): ");
                input = ReadLine().ToLower().Trim();
                
                if (input == "yes" || input == "y")
                    hasAmpedBloodAwakeningEffect = true;
                else if (input == "no" || input == "n")
                    hasAmpedBloodAwakeningEffect = false;
                else if (input == "back")
                    ReplaceValue("blood awakening");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputMailOfHellfire()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Mail of Hellfire: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Mail of Hellfire: Increases raw damage and decreases defense while the player uses the Red\n" +
                              "                   Swap Scroll. Also increaes elemental damage and decreases elemental\n" +
                              "                   resistances while the player uses the Blue Swap Scroll.\n\n" +
                              " Red Scroll:                Blue Scroll:\n" +
                              " Lvl 1: +15 raw damage.     Lvl 1: +5% elemental damage.\n" +
                              " Lvl 2: +25 raw damage.     Lvl 2: +10% elemtnal damage.\n" +
                              " Lvl 3: +30 raw damage.     Lvl 3: +20% elemental damage.\n");
                }
                else if (input == "back" && bloodAwakening > 0)
                    ReplaceValue("amplified blood awakening effect");
                else if (input == "back")
                    ReplaceValue("blood awakening");
            } while (!byte.TryParse(input, out mailOfHellfire) || mailOfHellfire < 0 || mailOfHellfire > 3);
        }

        static void InputFrostCraft()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Frost Craft Gauge (0-4): ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Frost Craft: Increases raw and elemental damage based on how full the Frostcraft gauge is when\n" +
                              "              the player lands an attack. This gauge fills passivley, but its speed will increase\n" +
                              "              when thier weapon is sheathed. Note, do not enter what the Frostcraft level is.\n" +
                              "              Instead just enter how full the Frostcraft gauge is.\n" +
                              " Gauge 1: Doesn't increase damage.\n" +
                              " Gauge 2: +5% raw and elemental damage.\n" +
                              " Gauge 3: +20% raw and elemental damage.\n" +
                              " Gauge 4: +30% raw and elemental damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("mail of hellfire");
            } while (!byte.TryParse(input, out frostCraftGauge) || frostCraftGauge < 0 || frostCraftGauge > 4);
        }

        static void InputBloodlust()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Bloodlust: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Bloodlust: When the player contracts the Frenzy Virus, their health is slowly reduced, but\n" +
                              "            in exchange, raw damage, elemental damage, and evasion increases, while stamina\n" +
                              "            use is reduced. They also gain affinity when they recover from the virus, but\n" +
                              "            the calculator will not include this.\n" +
                              " Lvl 1: +10 raw damage and +5 elemental damage. +20% affinity once recovered.\n" +
                              " Lvl 2: +15 raw damage and +7 elemental damage. +25% affinity once recovered.\n" +
                              " Lvl 3: +20 raw damage and +10 elemental damage. +25% affinity once recovered.\n");
                }
                else if (input == "back")
                    ReplaceValue("frostcraft");
            } while (!byte.TryParse(input, out bloodlust) || bloodlust < 0 || bloodlust > 3);
        }

        static void InputBurst()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Burst: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Burst: Increases raw and elemental damage after the player lands multiple attacks. The\n" +
                              "        number of attacks required to activate this skill depends on which weapon they use.\n" +
                              "        For maximum damage, the calculator will assume the player has already landed the\n" +
                              "        required number of attacks.\n" +
                              " Lvl 1: +10 raw damage and +8 elemental damage.\n" +
                              " Lvl 2: +12 raw damage and +10 elemental damage.\n" +
                              " Lvl 3: +15 raw damage and +15 elemental damage.\n");
                }
                else if (input == "back")
                    ReplaceValue("bloodlust");
            } while (!byte.TryParse(input, out burst) || burst < 0 || burst > 3);
        }

        static void InputInspiration()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Red;
                Write(" Inspiration (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasInspiration = true;
                else if (input == "no" || input == "n")
                    hasInspiration = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Inspiration: Increases raw damage by +10 after the player activates an effect that will\n" +
                              "              affect other players within a certain area, including Hunting Horn Songs.\n");
                }
                else if (input == "back")
                    ReplaceValue("burst");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputStormsoul()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Cyan;
                Write(" Stormsoul: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Stormsoul: Increases thunder and dragon elemental damage. Also increases any naturally\n" +
                              "            occurring armor skills (not those gained from gems or a talisman), but this\n" +
                              "            is not included in the calculator.\n");
                }
                else if (input == "back")
                    ReplaceValue("inspiration");
            } while (!byte.TryParse(input, out stormsoul) || stormsoul < 0 || stormsoul > 5);
        }
       
        static void InputDragonConversion()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Cyan;
                Write(" Dragon Conversion: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Dragon Conversion: Increases all elemental resistances. When the player uses the Red Swap Scroll,\n" +
                              "                    their elemental resistances are set to 0 and converted into elemental damage.\n" +
                              "                    If they are using the Blue Swap Scroll, their elemental resistances are\n" +
                              "                    further increased.\n" +
                              "                    After level 1 the Blue Swap Scroll will also allow them to gain even more\n" +
                              "                    elemental resistance if they continuously attack a monster.\n" +
                              "                    To calculate the elemental damage increase in the Red Swap Scroll, the sum of\n" +
                              "                    the player's elemental resistances is multiplied by a conversion rate. If\n" +
                              "                    some resistances are negative, they will be ignored.\n\n" +
                              " Red Scroll:                                          Blue Scroll:\n" +
                              " Lvl 1: Conversion rate: 15% melee and 4% ranged.     Lvl 1: +3 to all elemental resistances.\n" +
                              " Lvl 2: Conversion rate: 15% melee and 4% ranged.     Lvl 2: +5 to all elemental resistances.\n" +
                              " Lvl 3: Conversion rate: 25% melee and 8% ranged.     Lvl 3: +10 to all elemental resistances.\n");
                }
                else if (input == "back")
                    ReplaceValue("stormsoul");
            } while (!byte.TryParse(input, out dragonConversion) || dragonConversion < 0 || dragonConversion > 3);

            if (dragonConversion > 0)
                InputTotalElementalResistance();
        }        

        static void InputTotalElementalResistance()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Total Elemental Resistance (ignore negatives): ");
                input = ReadLine().ToLower().Trim();

                if (input == "back")
                    ReplaceValue("dragon conversion");
            } while (!int.TryParse(input, out totalElementalResistance) || totalElementalResistance < 0);
        }

        static void InputStrife()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Cyan;
                Write(" Strife: ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Strife: Increases elemental damage and affinity in proportion to the length of the player's\n" +
                              "         red health gauge. This armor skill activates if they have at least some red health and\n" +
                              "         is amplified if 60% of their health bar is red. At level 3, they also gain infinite\n" +
                              "         stamina so long as they have that 60% red health.\n\n" +
                              " If they have any red health:                   At 60% red health\n" +
                              " Lvl 1: +5% elemental damage and affinity.      Lvl 1: +10% elemental damage and affinity.\n" +
                              " Lvl 2: +10% elemental damage and affinity.     Lvl 2: +15% elemental damage and affinity.\n" +
                              " Lvl 3: +15% elemental damage and affinity.     Lvl 3: +20% elemental damage and affinity.\n");
                }
                else if (input == "back" && dragonConversion > 0)
                    ReplaceValue("total elemental resistance");
                else if (input == "back")
                    ReplaceValue("dragon conversion");
            } while (!byte.TryParse(input, out strife) || strife < 0 || strife > 3);        
            
            if (strife > 0)
                InputAmpedStrifeEffect();
        }

        static void InputAmpedStrifeEffect()
        {
            string input;

            do
            {
                ResetColor();
                Write(" Amplified Strife effect (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasAmpedStrifeEffect = true;
                else if (input == "no" || input == "n")
                    hasAmpedStrifeEffect = false;
                else if (input == "back")
                    ReplaceValue("strife");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputElementalExploit()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Cyan;
                Write(" Elemental Exploit (Skill): ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Elemental Exploit (Skill): Increases elemental damage when the player lands attacks on a monster\n" +
                              "                            part that is weak to the element that the player is using. A monster part\n" +
                              "                            is considered weak if it has an elemental hitzone of at least 20% or\n" +
                              "                            greater. This damage increase will not appear in the Status Menu.\n" +
                              " Lvl 1: +10% elemental damage.\n" +
                              " Lvl 2: +12.5% elemental damage.\n" +
                              " Lvl 3: +15% elemental damage.\n");
                }
                else if (input == "back" && strife > 0)
                    ReplaceValue("amplified strife effect");
                else if (input == "back")
                    ReplaceValue("strife");
            } while (!byte.TryParse(input, out elementalExploit) || elementalExploit < 0 || elementalExploit > 3);
        }
        #endregion

        #region Rampage Skill Input
        static void InputBrutalStrike()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Brutal Strike (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasBrutalStrike = true;
                else if (input == "no" || input == "n")
                    hasBrutalStrike = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Brutal Strike: Increases raw damage by +50% when the player lands a negative critical attack.\n" +
                              "                This damage increase will not appear in the Status Menu.\n");
                }
                else if (input == "back" && includeSunbreakArmorSkills)
                    ReplaceValue("elemental exploit skill");
                else if (input == "back")
                    ReplaceValue("critical boost");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputDullingStrike()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Dulling Strike (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasDullingStrike = true;
                else if (input == "no" || input == "n")
                    hasDullingStrike = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Dulling Strike: Gives the player a 10% chance of dealing +20% raw damage if they have\n" +
                              "                 green sharpness or lower. This damage increase will not appear\n" +
                              "                 in the Status Menu\n");
                }
                else if (input == "back")
                    ReplaceValue("brutal strike");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputElementalExploitRampage()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Elemental Exploit (Rampage) (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasElementalExploitRampage = true;
                else if (input == "no" || input == "n")
                    hasElementalExploitRampage = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Elemental Exploit (Rampage): Increases elemental damage by +30% when the player lands attacks on a\n" +
                              "                              monster part that is weak to the element that the player is using.\n" +
                              "                              A monster part is considered weak if it has an elemental hitzone of at\n" +
                              "                              least 25% or greater. This damage increase will not appear in the\n" +
                              "                              Status Menu.\n");
                }
                else if (input == "back" && !weaponType.Contains("bow"))
                    ReplaceValue("dulling strike");
                else if (input == "back")
                    ReplaceValue("brutal strike");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputSilkbindBoost()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Silkbind Boost (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasSilkbindBoost = true;
                else if (input == "no" || input == "n")
                    hasSilkbindBoost = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Silkbind Boost: Increases raw damage by +10% when the player performs a silkbind attack.\n" +
                              "                 This damage increase will not appear in the Status Menu.\n");
                }
                else if (input == "back")
                    ReplaceValue("elemental exploit rampage");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }
        #endregion

        #region Buff Input
        static void InputPowercharm()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Powercharm (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasPowercharm = true;
                else if (input == "no" || input == "n")
                    hasPowercharm = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Powercharm: Is an item the player may put into their inventory that increases raw damage by +6.\n" +
                              "             Stacks with Powertalon\n");
                }
                else if (input == "back")
                    ReplaceValue("silkbind boost");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputPowertalon()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Powertalon (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasPowerTalon = true;
                else if (input == "no" || input == "n")
                    hasPowerTalon = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Powertalon: Is an item the player may put into thier inventory that increases raw damage by +9.\n" +
                              "             Stacks with Powercharm.\n");
                }
                else if (input == "back")
                    ReplaceValue("powercharm");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputMightSeed()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Might Seed (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasMightSeed = true;
                else if (input == "no" || input == "n")
                    hasMightSeed = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Might Seed: Is a consumable that increases raw damage by +10 for 180 seconds.\n");
                }
                else if (input == "back")
                    ReplaceValue("powertalon");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputDemonPowder()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Demon Powder (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasDemonPowder = true;
                else if (input == "no" || input == "n")
                    hasDemonPowder = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Demon Powder: Is a consumable that increases raw damage by +10 for 180 seconds.\n" +
                              "               Affects other players around the user.\n");
                }
                else if (input == "back")
                    ReplaceValue("might seed");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputDemondrugType()
        {
            string input;

            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("\n Demondrug type:\n" +
                      " 0. None\n" +
                      " 1. Demondrug\n" +
                      " 2. Mega Demondrug\n");

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Demondrug type (0-2): ");
                input = ReadLine().ToLower().Trim();

                switch (input)
                {
                    case "0":
                    case "1":
                    case "2":
                        demondrugType = input;
                        break;
                    case "?":
                        ResetColor();
                        WriteLine("\n Demondrug: Is a consumable that increases raw damage by +5. This lasts until the\n" +
                                  "            player faints. Will not stack with Mega Demondrug.\n\n" +
                                  " Mega DemonDrug: Is a consumable that increases raw damage by +7. This lasts until\n" +
                                  "                 the player faints. Will take priority over the Demondrug buff.\n");
                        break;
                    case "back":
                        ReplaceValue("demon powder");
                        break;
                }
            } while (input == "" || input != "0" && input != "1" && input != "2");
        }

        static void InputPalicoPowerDrum()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Palico Power Drum (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasPowerDrum = true;
                else if (input == "no" || input == "n")
                    hasPowerDrum = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Palico Power Drum: Increases raw damage by +5% for 120 seconds.\n");
                }
                else if (input == "back")
                    ReplaceValue("demondrug type");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputRousingRoar()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Rousing Roar (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasRousingRoar = true;
                else if (input == "no" || input == "n")
                    hasRousingRoar = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Rousing Roar: Increases affinity by +30% for 150 seconds.\n");
                }
                else if (input == "back")
                    ReplaceValue("palico power drum");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputDangoBooster()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Dango Booster Level (0-4): ");
                input = ReadLine().ToLower().Trim();

                if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Dango Booster: Can be acquired by eating the Mint-iature Dango.\n" +
                              " Lvl 1: +6 raw damage for 10 minutes.\n" +
                              " Lvl 2: +9 raw damage for 10 minutes.\n" +
                              " Lvl 3: +12 raw damage for 8 minutes.\n" +
                              " Lvl 4; +15 raw damage for 8 minutes.\n");
                }
                else if (input == "back")
                    ReplaceValue("rousing roar");
            } while (!byte.TryParse(input, out dangoBooster) || dangoBooster < 0 || dangoBooster > 4);
        }

        static void InputWaterblight()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Waterblight (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasWaterblight = true;
                else if (input == "no" || input == "n")
                    hasWaterblight = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Waterblight: Mudbeetles can be used to inflict this on monsters, after which the value\n" +
                              "              of their raw hitzones will increase. The amount of this increase depends on\n" +
                              "              their origional value.\n" +
                              " Raw Hitzone >= 60%:      +3% increase.\n" +
                              " 60% > Raw Hitzone > 25%: (raw hitzone * 0.0625) + 0.25 increase\n" +
                              " Raw Hitzone <= 25%:      (raw hitzone * 0.6) + 0.25 increase.\n");
                }
                else if (input == "back")
                    ReplaceValue("dango booster");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputAttackUpSong()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Attack Up song effect (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasAttackUpSong = true;
                else if (input == "no" || input == "n")
                    hasAttackUpSong = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Attack Up song effect: Increases raw damage by +10% for 60 seconds.\n");
                }
                else if (input == "back")
                    ReplaceValue("waterblight");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputElementalUpSong()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Elemental Up song effect (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                    hasElementalUpSong = true;
                else if (input == "no" || input == "n")
                    hasElementalUpSong = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("\n Elemental Up song effect: Increases elemental damage by +10% for 60 seconds.\n");
                }
                else if (input == "back")
                    ReplaceValue("attack up song effect");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }

        static void InputAffinityUpSong()
        {
            string input;

            do
            {
                ForegroundColor = ConsoleColor.Yellow;
                Write(" Affinity Up song effect (y/n): ");
                input = ReadLine().ToLower().Trim();

                if (input == "yes" || input == "y")
                   hasAffinityUpSong = true;
                else if (input == "no" || input == "n")
                    hasAffinityUpSong = false;
                else if (input == "?")
                {
                    ResetColor();
                    WriteLine("Affinity Up song effect: Increases affinity by +20% for 60 seconds.\n");
                }
                else if (input == "back")
                    ReplaceValue("elemental up song effect");
            } while (input == "" || input != "yes" && input != "y" && input != "no" && input != "n");
        }
        #endregion

        #region Calculations
        /************************************************************************************************************
         * Phase 1 Calculations: The goal of this function is to calculate the raw and elemental damage found in the
         *                       Status Menu. It may not include all armor skills or buffs.
         *                       
         * How the calculations are done - Basically, if an effect increases damage by a percentage, it will get
         *                                 immediatley multiplied to the raw or elemental variables. If an effect
         *                                 increases damage by a flat rate, then it will get added to the raw or
         *                                 elemental flat bonus damage variables. Once all of the percentage based
         *                                 effects have been included, then  the bonus damage varibles will be added
         *                                 to the raw and elemental variables.
         ***********************************************************************************************************/
        static void CalculatePhase1Damage()
        {
            //Assigning values from the user's input.
            raw = enteredRaw;
            ele = enteredEle;
            affinity = enteredAffinity;
            rawHitzone = enteredRawHitzone;
            eleHitzone = enteredEleHitzone;
            rawFlatBonusDmg = enteredRawFlatBonusDmg;
            eleFlatBonusDmg = enteredEleFlatBonusDmg;

            //Assigning values for the raw and elemental critical percentages. These set the damage that
            //critical hits increase damage by. The armor skill Critical Boost is used here too.
            rawCritPercent = 0.25f;
            eleCritPercent = 0f;
            rawCritPercent += (float)(criticalBoost * 0.05);
            eleCritPercent += (float)(criticalElement * 0.05);

            //Only some of the weapon specific buffs get applied before the Status Menu calculations.
            if (weaponType == "greatsword" || weaponType == "longsword" || weaponType == "dual blades" ||
                weaponType == "hunting horn" || weaponType == "insect glaive")
                CalculateWeaponBuffDamage();

            CalculatePhase1ArmorSkillDamage();

            //Buffs
            if (hasPowercharm)
                rawFlatBonusDmg += 6;

            if (hasPowerTalon)
                rawFlatBonusDmg += 9;

            if (hasMightSeed)
                rawFlatBonusDmg += 10;

            if (hasDemonPowder)
                rawFlatBonusDmg += 10;

            if (demondrugType == "1")
                rawFlatBonusDmg += 5;
            else if (demondrugType == "2")
                rawFlatBonusDmg += 7;

            if (hasPowerDrum)
                raw *= 1.05;

            if (hasRousingRoar)
                affinity += 0.3f;

            if (dangoBooster > 0)
                rawFlatBonusDmg += dangoBooster * 3 + 3;

            //Hunting horn songs
            if (hasAttackUpSong)
                raw *= 1.1;

            if (hasElementalUpSong && ele > 0)
                ele *= 1.2;

            if (hasAffinityUpSong)
                affinity += 0.2f;

            raw *= rawPercentBonusDmg;
            ele *= elePercentBonusDmg;
            raw += rawFlatBonusDmg;
            ele += eleFlatBonusDmg;

            //Round raw and elemental damage down and save their current values
            //into their Status Menu variables.
            raw = Math.Floor(raw);
            ele = Math.Floor(ele);
            statusMenuRaw = (int)raw;
            statusMenuEle = (int)ele;
        }
   
        static void CalculatePhase1ArmorSkillDamage() 
        {
            switch (attackBoost)
            {
                case 1:
                case 2:
                case 3:
                    rawFlatBonusDmg += attackBoost * 3;
                    break;
                case 4:
                    raw *= 1.05;
                    rawFlatBonusDmg += 7;
                    break;
                case 5:
                    raw *= 1.06;
                    rawFlatBonusDmg += 8;
                    break;
                case 6:
                    raw *= 1.08;
                    rawFlatBonusDmg += 9;
                    break;
                case 7:
                    raw *= 1.1;
                    rawFlatBonusDmg += 10;
                    break;
            }

            switch (bludgeoner)
            {
                case 1:
                    if (enteredSharpness == "yellow" || enteredSharpness == "orange" ||
                        enteredSharpness == "red")
                        raw *= 1.05;
                    break;
                case 2:
                    if (enteredSharpness == "yellow" || enteredSharpness == "orange" ||
                        enteredSharpness == "red")
                        raw *= 1.1;
                    break;
                case 3:
                    if (enteredSharpness == "green" || enteredSharpness == "yellow" ||
                        enteredSharpness == "orange" || enteredSharpness == "red")
                        raw *= 1.1;
                    break;
            }

            switch (heroics)
            {
                case 2:
                case 3:
                    raw *= 1.05;
                    break;
                case 4:
                    raw *= 1.1;
                    break;
                case 5:
                    raw *= 1.3;
                    break;
            }

            switch (dragonheart)
            {
                case 4:
                    raw *= 1.05;
                    break;
                case 5:
                    raw *= 1.1;
                    break;
            }

            if (offensiveGuard > 0)
                raw *= 1 + (offensiveGuard * 0.05);

            if (hasFortify)
                raw *= 1.2;

            if (hasHornMaestro)
                raw *= 1.1;
                   
            rawFlatBonusDmg += (agitator * 4);
            switch (agitator)
            {
                case 1:
                    affinity += 0.03f;
                    break;
                case 2:
                    affinity += 0.05f;
                    break;
                case 3:
                    affinity += 0.07f;
                    break;
                case 4:
                    affinity += 0.1f;
                    break;
                case 5:
                    affinity += 0.15f;
                    break;
            }

            switch (peakPerformance)
            {
                case 1:
                case 2:
                    rawFlatBonusDmg += peakPerformance * 5;
                    break;
                case 3:
                    rawFlatBonusDmg += 20;
                    break;
            }

            switch (resuscitate)
            {
                case 1:
                case 2:
                    rawFlatBonusDmg += resuscitate * 5;
                    break;
                case 3:
                    rawFlatBonusDmg += 20;
                    break;
            }

            rawFlatBonusDmg += (resentment * 5);

            switch (counterstrike)
            {
                case 1:
                    rawFlatBonusDmg += 10;
                    break;
                case 2:
                    rawFlatBonusDmg += 15;
                    break;
                case 3:
                    rawFlatBonusDmg += 25;
                    break;
            }

            switch (punishingDraw)
            {
                case 1:
                    rawFlatBonusDmg += 3;
                    break;
                case 2:
                    rawFlatBonusDmg += 5;
                    break;
                case 3:
                    rawFlatBonusDmg += 7;
                    break;
            }

            switch (coalescence)
            {
                case 1:
                    rawFlatBonusDmg += 12;
                    if (ele > 0)
                        eleFlatBonusDmg += 2;
                    break;
                case 2:
                    rawFlatBonusDmg += 15;
                    if (ele > 0)
                        eleFlatBonusDmg += 3;
                    break;
                case 3:
                    rawFlatBonusDmg += 18;
                    if (ele > 0)
                        eleFlatBonusDmg += 4;
                    break;
            }

            if (ele > 0)
            {
                switch (blightAttack)
                {
                    case 1:
                        eleFlatBonusDmg += 2;
                        break;
                    case 2:
                        eleFlatBonusDmg += 3;
                        break;
                    case 3:
                        ele *= 1.05;
                        eleFlatBonusDmg += 4;
                        break;
                    case 4:
                        ele *= 1.1;
                        eleFlatBonusDmg += 4;
                        break;
                    case 5:
                        ele *= 1.2;
                        eleFlatBonusDmg += 4;
                        break;
                }
            }          

            if (elderDragonBlessing > 1 && ele > 0)
                ele *= 1.1;
            else if (elderDragonBlessing > 0 && ele > 0)
                ele *= 1.05;

            if (criticalEye == 7)
                affinity += 0.4f;
            else if (criticalEye > 0)
                affinity += criticalEye * 0.05f;

            //Weakness exploit considers weak parts to have a raw hitzone of at least 45%.
            if (rawHitzone >= 0.45)
            {
                if (weaknessExploit == 3)
                    affinity += 0.5f;
                else if (weaknessExploit > 0)
                    affinity += weaknessExploit * 0.15f;
            }

            if (criticalDraw == 3)
                affinity += 0.6f;
            else if (criticalDraw > 0)
                affinity += criticalDraw * 0.15f;

            affinity += maximumMight * 0.1f;

            affinity += latentPower * 0.1f;

            //Sunbreak Armor Skills
            if (includeSunbreakArmorSkills)
            {
                //Will include all of the armor skills that will be applied if the swap scroll is red.
                if (swapScrollColor == "red")
                {
                    if (ele > 0)
                    {
                        switch (dereliction)
                        {
                            case 1:
                                eleFlatBonusDmg += 12;
                                break;
                            case 2:
                                eleFlatBonusDmg += 15;
                                break;
                            case 3:
                                eleFlatBonusDmg += 20;
                                break;
                        }

                        switch (dragonConversion)
                        {
                            case 1:
                            case 2:
                                if (!weaponType.Contains("bow"))
                                    eleFlatBonusDmg += (int)Math.Floor(totalElementalResistance * 0.15);
                                else
                                    eleFlatBonusDmg += (int)Math.Floor(totalElementalResistance * 0.04);
                                break;
                            case 3:
                                if (!weaponType.Contains("bow"))
                                    eleFlatBonusDmg += (int)Math.Floor(totalElementalResistance * 0.25);
                                else
                                    eleFlatBonusDmg += (int)Math.Floor(totalElementalResistance * 0.08);
                                break;
                        }
                    }
                   
                    switch (mailOfHellfire)
                    {
                        case 1:
                            rawFlatBonusDmg += 15;
                            break;
                        case 2:
                            rawFlatBonusDmg += 25;
                            break;
                        case 3:
                            rawFlatBonusDmg += 35;
                            break;
                    }
                }
                else //Will include all of the armor skills that will be applied if the swap scroll is blue.
                {
                    switch (dereliction)
                    {
                        case 1:
                            rawFlatBonusDmg += 25;
                            break;
                        case 2:
                            rawFlatBonusDmg += 30;
                            break;
                        case 3:
                            rawFlatBonusDmg += 35;
                            break;
                    }

                    if (ele > 0)
                    {
                        switch (mailOfHellfire)
                        {
                            case 1:
                            case 2:
                                ele *= 1 + (mailOfHellfire * 0.05f);
                                break;
                            case 3:
                                ele *= 1.2f;
                                break;
                        }
                    }                   
                }

                if (hasAmpedBloodAwakeningEffect)
                {
                    switch (bloodAwakening)
                    {
                        case 1:
                            rawFlatBonusDmg += 8;
                            if (ele > 0)
                                eleFlatBonusDmg += 8;
                            break;
                        case 2:
                            rawFlatBonusDmg += 15;
                            if (ele > 0)
                                eleFlatBonusDmg += 10;
                            break;
                        case 3:
                            rawFlatBonusDmg += 25;
                            if (ele > 0)
                                eleFlatBonusDmg += 15;
                            break;
                    }
                }
                else
                {
                    switch (bloodAwakening)
                    {
                        case 1:
                        case 2:
                            rawFlatBonusDmg += 4;
                            if (ele > 0)
                                eleFlatBonusDmg += 4;
                            break;
                        case 3:
                            rawFlatBonusDmg += 10;
                            if (ele > 0)
                                eleFlatBonusDmg += 8;
                            break;
                    }
                }

                if (hasInspiration)
                        rawFlatBonusDmg += 10;

                switch (adrenalineRush)
                {
                    case 1:
                    case 3:
                        rawFlatBonusDmg += adrenalineRush * 10;
                        break;
                    case 2:
                        rawFlatBonusDmg += 15;
                        break;
                }

                switch (bloodlust)
                {
                    case 1:
                        rawFlatBonusDmg += 10;
                        if (ele > 0)
                            eleFlatBonusDmg += 5;
                        break;
                    case 2:
                        rawFlatBonusDmg += 15;
                        if (ele > 0)
                            eleFlatBonusDmg += 7;
                        break;
                    case 3:
                        rawFlatBonusDmg += 20;
                        if (ele > 0)
                            eleFlatBonusDmg += 10;
                        break;
                }

                switch (burst)
                {
                    case 1:
                        rawFlatBonusDmg += 10;
                        if (ele > 0)
                            eleFlatBonusDmg += 8;
                        break;
                    case 2:
                        rawFlatBonusDmg += 12;
                        if (ele > 0)
                            eleFlatBonusDmg += 10;
                        break;
                    case 3:
                        rawFlatBonusDmg += 15;
                        if (ele > 0)
                            eleFlatBonusDmg += 15;
                        break;
                }

                if ((stormsoul == 4 || stormsoul == 5) && ele > 0)
                    ele *= 1.15;
                else if (stormsoul > 0 && ele > 0)
                    ele *= 1 + (stormsoul * 0.05);
               
                if (hasAmpedStrifeEffect)
                {
                    switch (strife)
                    {
                        case 1:
                            if (ele > 0)
                                ele *= 1.1;
                            affinity += 0.1f;
                            break;
                        case 2:
                            if (ele > 0)
                                ele *= 1.15;
                            affinity += 0.15f;
                            break;
                        case 3:
                            if (ele > 0)
                                ele *= 1.2;
                            affinity += 0.2f;
                            break;
                    }
                }
                else //Strife will assume that the player at least has some red health.
                {
                    if (ele > 0)
                        ele *= 1 + (strife * 0.05);
                    if (strife > 2)
                        affinity += 0.10f;
                    else
                        affinity += strife * 0.05f;                                       
                }
            }           
        }

        /**************************************************************************************
         * Phase 2 Calculations - Will calculate raw and elemental damage with the armor skills
         *                        and buffs that were not included in Phase 1.
         **************************************************************************************/
        static void CalculatePhase2Damage()
        {
            //Reseting these because Foray is the ONLY ARMOR SKILL THAT INCREASES DAMAGE
            //BY A FLAT RATE IN PHASE 2 AHHHHH.
            rawFlatBonusDmg = 0;
            eleFlatBonusDmg = 0;
           
            //Applying the weapon specific buffs for chargeblade and switchaxe.
            if (weaponType == "chargeblade" || weaponType == "switch axe")
                CalculateWeaponBuffDamage();

            CalculatePhase2ArmorSkillDamage();

            if ((hasElementalExploitRampage && eleHitzone >= 0.25) && ele > 0)
                ele *= 1.3;

            if (hasSilkbindBoost)
                raw *= 1.1;
         
            raw *= rawDmgMultiplier;
            ele *= eleDmgMultiplier;
            raw += rawFlatBonusDmg;
            ele += eleFlatBonusDmg;
        }

        static void CalculatePhase2ArmorSkillDamage()
        {
            if (rapidMorph > 0)
                raw *= 1 + ((rapidMorph - 1) * 0.1);

            switch (ammoTypeUp)
            {
                case 1:
                case 2:
                    raw *= 1 + (ammoTypeUp * 0.05);
                    break;
                case 3:
                    raw *= 1.20;
                    break;
            }

            if (specialAmmoBoost > 0)
                raw *= 1 + (specialAmmoBoost * 0.1);

            if (rawHitzone * rawSharpness < 0.45)
            {
                switch (mindsEye)
                {
                    case 1:
                        raw *= 1.1;
                        break;
                    case 2:
                        raw *= 1.15;
                        break;
                    case 3:
                        raw *= 1.3;
                        break;
                }
            }

            //Sunbreak Armor Skills
            if (includeSunbreakArmorSkills)
            {
                switch (sneakAttack)
                {
                    case 1:
                        raw *= 1.05;
                        break;
                    case 2:
                        raw *= 1.1;
                        break;
                    case 3:
                        raw *= 1.2;
                        break;
                }

                switch (buildupBoost)
                {
                    case 1:
                        raw *= 1.1;
                        break;
                    case 2:
                        raw *= 1.15;
                        break;
                    case 3:
                        raw *= 1.2;
                        break;
                }

                switch (foray)
                {
                    case 1:
                        rawFlatBonusDmg += 10;
                        break;
                    case 2:
                        rawFlatBonusDmg += 10;
                        affinity += 0.1f;
                        break;
                    case 3:
                        rawFlatBonusDmg += 15;
                        affinity += 0.2f;
                        break;
                }

                switch (frostCraftGauge)
                {
                    case 2:
                        raw *= 1.05;
                        if (ele > 0)
                            ele *= 1.05;
                        break;
                    case 3:
                        raw *= 1.2;
                        if (ele > 0)
                            ele *= 1.2;
                        break;
                    case 4:
                        raw *= 1.3;
                        if (ele > 0)
                            ele *= 1.3;
                        break;
                }

                if (eleHitzone >= 0.2 && ele > 0)
                {
                    switch (elementalExploit)
                    {
                        case 1:
                            ele *= 1.1;
                            break;
                        case 2:
                            ele *= 1.125;
                            break;
                        case 3:
                            ele *= 1.15;
                            break;
                    }
                }
                
            }
        }

        /******************************************************************************************************
         * Calculate Weapon Buff Damage - For simplicity, all of the weapon specific buffs are stuffed here.
         *                                This function will be called both in Phase 1 and 2, but in each
         *                                case, it will only be called if the player is using a certain weapon.
         ******************************************************************************************************/
        static void CalculateWeaponBuffDamage()
        {
            if (weaponType == "greatsword" && weaponTypeBuff == "power sheathe")
            {
                raw *= 1.1;
            }
            else if (weaponType == "longsword")
            {
                switch (weaponTypeBuff)
                {
                    case "spirit gauge level 1":
                        raw *= 1.04;
                        if (ele > 0)
                            ele *= 1.04;
                        break;
                    case "spirit gauge level 2":
                        raw *= 1.08;
                        if (ele > 0)
                            ele *= 1.08;
                        break;
                    case "spirit gauge level 3":
                        raw *= 1.12;
                        if (ele > 0)
                            ele *= 1.12;
                        break;
                }
            }
            else if (weaponType == "dual blades")
            {
                switch (weaponTypeBuff)
                {
                    case "demon mode":
                        if (ele > 0)
                            ele *= 1.35;
                        break;
                    case "feral demon mode":
                        raw *= 1.2;
                        break;
                    case "archdemon mode":
                        if (ele > 0)
                            ele *= 1.2;
                        break;
                }
            }
            else if (weaponType == "hunting horn" && weaponTypeBuff == "infernal melody")
            {
                raw *= 1.2;
            }
            else if (weaponType == "switch axe")
            {
                switch (weaponTypeBuff)
                {
                    case "power phial":
                        raw *= 1.15;
                        break;
                    case "elemental phial":
                        if (ele > 0)
                            ele *= 1.45;
                        break;
                }
            }
            else if (weaponType == "chargeblade" && weaponTypeBuff == "charged shield")
            {
                raw *= 1.1;
            }
            else if (weaponType == "insect glaive")
            {
                switch (weaponTypeBuff)
                {
                    case "red and white kinsects active":
                        raw *= 1.1;
                        break;
                    case "all kinsects active":
                        raw *= 1.2;
                        break;
                }
            }
        }

        /*******************************************************************************************************
         * Phase 3 Calculations - Will calculate total raw and elemental damage, including critical damage with
         *                        postive and negative affinity. It will also include affinity fixing as well as
         *                        waterblight adjustments to the raw hitzone value.
         *******************************************************************************************************/
        static void CalculatePhase3TotalDamage()
        {
            //Prevents affinity from going past -100% or 100%.
            if (affinity > 1)
                affinity = 1;
            else if (affinity < -1)
                affinity = -1;

            if (hasWaterblight)
            {
                if (rawHitzone >= 0.6)
                    rawHitzone += 0.03f;
                else if (rawHitzone > 0.25)
                    rawHitzone = rawHitzone * 0.625f + 0.25f;
                else if (rawHitzone <= 0.25)
                    rawHitzone = rawHitzone * 0.6f + 0.25f;
            }

            //Hey refer to just these if you are trying to introduce noobs to the calculation process.
            totalRaw = raw * rawSharpness * motionValue * rawHitzone;
            totalEle = ele * eleSharpness * 1 * eleHitzone;

            if (affinity >= 0)
                totalRawCrit = totalRaw * (1 + rawCritPercent);
            else
                totalRawCrit = totalRaw * (1 - 0.25);

            //Elemental damage will not be affected by negative affinity at all.
            totalEleCrit = totalEle * (1 + eleCritPercent);

            totalDmg = Math.Round(totalRaw) + Math.Round(totalEle);
            totalDmgCrit = Math.Round(totalRawCrit) + Math.Round(totalEleCrit);
        }
        #endregion

        /*******************************************************************************************************
         * Display Total Damage - Will display all of the damage values that can be found in the Status Menu and
         *                        ones that will lead to the total damage values. The Affinity value will
         *                        represent the total affinity once all effects have been applied, this means it
         *                        may not reflect what is in the Status Menu.
         *                        
         *                        Also, because I am lazy, Brutual Strike and Dulling Strike damage gets
         *                        calculated here.
         *******************************************************************************************************/
        static void DisplayTotalDamage()
        {
            //Will capitalize each word in the weapon type and sharpness names.
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
           
            ForegroundColor = ConsoleColor.DarkGreen;
            WriteLine("\n            Weapon Type: " + textInfo.ToTitleCase(weaponType));
            WriteLine("             Raw Damage: " + statusMenuRaw);
            WriteLine("       Elemental Damage: " + statusMenuEle);
            WriteLine("              Sharpness: " + textInfo.ToTitleCase(enteredSharpness));
            WriteLine("               Affinity: " + affinity * 100 + "%");

            ForegroundColor = ConsoleColor.Green;
            WriteLine("\n              Total Raw: " + Math.Round(totalRaw, 2));
            WriteLine("       Total Raw (Crit): " + Math.Round(totalRawCrit, 2));

            if (hasBrutalStrike)
                WriteLine("          Brutal Strike: " + Math.Round(totalRaw * 1.5, 2));   
            else if (hasDullingStrike)
            {
                WriteLine("         Dulling Strike: " + Math.Round(totalRaw * 1.2, 2));
                WriteLine("  Dulling Strike (Crit): " + Math.Round(totalRawCrit * 1.2, 2));
            }

            WriteLine("        Total Elemental: " + Math.Round(totalEle, 2));
            WriteLine(" Total Elemental (Crit): " + Math.Round(totalEleCrit, 2));           
            WriteLine("           Total Damage: " + totalDmg);
            WriteLine("    Total Damage (Crit): " + totalDmgCrit);

            if (hasBrutalStrike)
                WriteLine("         Brutal Strike: " + Math.Round((totalRaw * 1.5) + Math.Round(totalEle)));
            else if (hasDullingStrike)
            {
                WriteLine("         Dulling Strike: " + Math.Round((totalRaw * 1.2) + Math.Round(totalEle)));
                WriteLine("  Dulling Strike (Crit): " + Math.Round((totalRawCrit * 1.2) + Math.Round(totalEleCrit)));
            }
            WriteLine();
            textInfo = null; //Puts the object in the Garbage Collector for deletion.
        }

        /************************************************************************************************************
         * Replace Value Function - This function will take a string and use it to find an input function to execute.
         * 
         *                          At any point in the input process, the player may use the command "back" to 
         *                          go to the previous question and change its value. But they cannot change whether
         *                          or not they want to include Sunbreak skills or their weapon type.
         *                          
         *                          Also, once calculations are done, the player may enter "replace" and then the
         *                          name of a value to immediatley go to that function and change its value. Then,
         *                          the calculator will redo the calculations with that change.                  
         ***********************************************************************************************************/
        static bool ReplaceValue(string input)
        {
            bool foundValue = true;
            string errorMessage = "";

            ResetColor();
            WriteLine("\n Replace request acknowledged...");

            switch (input)
            {
                case "weapon buff":
                case "weapon type buff":
                    InputWeaponTypeBuff();
                    break;
                case "swap scroll":
                case "scroll color":
                case "swap scroll color":
                    if (includeSunbreakArmorSkills)
                        InputSwapScrollColor();
                    else
                    {
                        errorMessage = " Swap Scroll Color is a part of the Sunbreak DLC and cannot be included in this calculation.";
                        foundValue = false;
                    }                       
                    break;
                case "raw":
                case "raw damage":
                    InputRawDamage();
                    break;
                case "elemental":
                case "elemental damage":
                    InputElementalDamage();
                    break;
                case "sharpness":
                    if (!weaponType.Contains("bow"))
                        InputSharpness();
                    else
                    {
                        errorMessage = " Sharpness cannot be included when using a ranged weapon.";
                        foundValue = false;
                    }
                    break;
                case "affinity":
                    InputAffinity();
                    break;
                case "motion value":
                    InputMotionValue();
                    break;
                case "raw hitzone":
                    InputRawHitzone();
                    break;
                case "elemental hitzone":
                    InputElementalHitzone();
                    break;
                case "raw flat bonus":
                case "raw flat bonus damage":
                    InputRawFlatBonusDamage();
                    break;
                case "raw percent":
                case "raw percent bonus":
                case "raw percent bonus damage":
                    InputRawPercentBonusDamage();
                    break;
                case "elemental flat":
                case "elemental flat bonus":
                case "elemental flat bonus damage":
                    InputElementalFlatBonusDamage();
                    break;
                case "elemental percent":
                case "elemental percent bonus":
                case "elemental percent bonus damage":
                    InputElementalPercentBonusDamage();
                    break;
                case "raw multipler":
                case "raw damage multiplier":
                    InputRawDamageMultipler();
                    break;
                case "elemental multiplier":
                case "elemental damage multiplier":
                    InputRawDamageMultipler();
                    break;
                case "horn maestro":
                    if (weaponType == "hunting horn")
                        InputHornMaestro();
                    else
                    {
                        errorMessage = " Horn Maestro can only be included when using Hunting Horn.";
                        foundValue = false;
                    }
                    break;
                case "rapid morph":
                    if (weaponType == "switch axe" || weaponType == "chargeblade")
                        InputRapidMorph();
                    else
                    {
                        errorMessage = " Rapid Moprh can only be included when using Switch Axe or Chargeblade.";
                        foundValue = false;
                    }
                    break;
                case "offensive guard":
                    if (weaponType == "greatsword" || weaponType == "chargeblade" ||
                        weaponType == "sword & shield" || weaponType == "lance" ||
                        weaponType == "gunlance")
                        InputOffensiveGuard();
                    else
                    {
                        errorMessage = " Offensive guard can only be included when using Greatsword, Chargeblade" +
                                       " Sword & Shield, Lance, and Gunlance.";
                        foundValue = false;
                    }
                    break;
                case "ammo up":
                case "ammo boost":
                case "ammo type up":
                case "ammo type boost":
                    if (weaponType.Contains("bow"))
                        InputAmmoTypeUp();
                    else
                    {
                        errorMessage = " Ammo Type Up can only be included for ranged weapons.";
                        foundValue = false;
                    }
                    break;
                case "special ammo":
                case "special ammo boost":
                    if (weaponType.Contains("bow"))
                        InputSpecialAmmoBoost();
                    else
                    {
                        errorMessage = " Special Ammo Boost can only be included for ranged weapons.";
                        foundValue = false;
                    }
                    break;
                case "attack boost":
                    InputAttackBoost();
                    break;
                case "bludgeoner":
                    InputBludgeoner();
                    break;
                case "mind eye":
                case "minds eye":
                case "mind's eye":
                    InputMindsEye();
                    break;
                case "heroics":
                    InputHeroics();
                    break;
                case "dragonheart":
                case "dragon heart":
                    InputDragonheart();
                    break;
                case "agitator":
                    InputAgitator();
                    break;
                case "peak performance":
                    InputPeakPerformance();
                    break;
                case "resuscitate":
                    InputResuscitate();
                    break;
                case "resentment":
                    InputResentment();
                    break;
                case "counter strike":
                case "counterstrike":
                    InputCounterstrike();
                    break;
                case "punishing draw":
                    InputPunishingDraw();
                    break;
                case "coalescence":
                    InputCoalescence();
                    break;
                case "fortify":
                    InputFortify();
                    break;
                case "blight attack":
                    InputBlightAttack();
                    break;
                case "kushala blessing":
                case "kushalas blessing":
                case "kushala's blessing":
                case "teostra blessing":
                case "teostras blessing":
                case "teostra's blessing":
                case "kushala or teostra blessing":
                case "kushala's or teostra's blessing":
                case "elder dragon blessing":
                case "elder dragon's blessing":
                    InputElderDragonBlessing();
                    break;
                case "critical element":
                    InputCriticalElement();
                    break;
                case "critical eye":
                    InputCriticalEye();
                    break;
                case "weakness exploit":
                    InputWeaknessExploit();
                    break;
                case "critical draw":
                    InputCriticalDraw();
                    break;
                case "maximum might":
                    InputMaximumMight();
                    break;
                case "latent power":
                    InputLatentPower();
                    break;
                case "critical boost":
                    InputCriticalBoost();
                    break;
                case "brutal strike":
                    InputBrutalStrike();
                    break;
                case "dulling strike":
                    if (!weaponType.Contains("bow"))
                        InputDullingStrike();
                    else
                    {
                        errorMessage = " Dulling Strike cannot be included for ranged weapons.";
                        foundValue = false;
                    }
                    break;
                case "elemental exploit rampage":
                    InputElementalExploitRampage();
                    break;
                case "silkbind boost":
                    InputSilkbindBoost();
                    break;
                case "power charm":
                case "powercharm":
                    InputPowercharm();
                    break;
                case "power talon":
                case "powertalon":
                    InputPowertalon();
                    break;
                case "mightseed":
                case "might seed":
                    InputMightSeed();
                    break;
                case "demon powder":
                    InputDemonPowder();
                    break;
                case "demon drug":
                case "demondrug":
                case "mega demon drug":
                case "mega demondrug":
                case "demon drug type":
                case "demondrug type":
                    InputDemondrugType();
                    break;
                case "power drum":
                case "palico drum":
                case "palico power drum":
                    InputPalicoPowerDrum();
                    break;
                case "rousing roar":
                    InputRousingRoar();
                    break;
                case "dango booster":
                    InputDangoBooster();
                    break;
                case "water blight":
                case "waterblight":
                    InputWaterblight();
                    break;
                case "attack song":
                case "attack up song":
                case "attack song effect":
                case "attack up song effect":
                    InputAttackUpSong();
                    break;
                case "elemental song":
                case "elemental up song":
                case "elemental song effect":
                case "elemental up song effect":
                    InputElementalUpSong();
                    break;
                case "affinity song":
                case "affinity up song":
                case "affinity song effect":
                case "affinity up song effect":
                    InputAffinityUpSong();
                    break;
                default:
                    if (foundValue)
                    {
                        errorMessage = " Replace request failed...";
                        foundValue = false;
                    }                    
                    break;
            }

            if (includeSunbreakArmorSkills && !foundValue)
            {
                foundValue = true;
                switch (input)
                {
                    case "sneak attack":
                        InputSneakAttack();
                        break;
                    case "build up boost":
                    case "buildup boost":
                        InputBuildupBoost();
                        break;
                    case "adrenaline rush":
                        InputAdrenalineRush();
                        break;
                    case "foray":
                        InputForay();
                        break;
                    case "dereliction":
                        InputDereliction();
                        break;
                    case "blood awakening":
                        InputBloodAwakening();
                        break;
                    case "amped blood awakening":
                    case "amped blood awakening effect":
                    case "amplified blood awakening":
                    case "amplified blood awakening effect":
                        if (bloodAwakening > 0)
                            InputAmpedBloodAwakeningEffect();
                        else
                        {
                            errorMessage = " Blood Awakening must be assigned first.";
                            foundValue = false;
                        }
                        break;
                    case "mail of hellfire":
                        InputMailOfHellfire();
                        break;
                    case "frost craft":
                    case "frostcraft":
                        InputFrostCraft();
                        break;
                    case "blood lust":
                    case "bloodlust":
                        InputBloodlust();
                        break;
                    case "burst":
                        InputBurst();
                        break;
                    case "inspiration":
                        InputInspiration();
                        break;
                    case "storm soul":
                    case "stormsoul":
                        InputStormsoul();
                        break;
                    case "dragon conversion":
                        InputDragonConversion();
                        break;
                    case "total resistance":
                    case "elemetnal resistance":
                    case "total elemental resistance":
                        InputTotalElementalResistance();
                        break;
                    case "strife":
                        InputStrife();
                        break;
                    case "amped strife":
                    case "amped strife effect":
                    case "amplified strife":
                    case "amplified strife effect":
                        if (strife > 0)
                            InputAmpedStrifeEffect();
                        else
                        {
                            errorMessage = " Strife must be assigned first.";
                            foundValue = false;
                        }
                        break;
                    case "elemental exploit skill":
                        InputElementalExploit();
                        break;
                    default:
                        if (foundValue)
                        {
                            errorMessage = " Replace request failed...";
                            foundValue = false;
                        }
                        break;
                }
            }
           
            if (!foundValue)
            {
                ResetColor();
                WriteLine(errorMessage + "\n");
            }           
            return foundValue;
        }
    }
}