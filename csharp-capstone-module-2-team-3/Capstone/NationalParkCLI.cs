using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using Capstone.DAL;
using System.Globalization;

namespace Capstone.Models
{
    public class NationalParkCLI
    {

        const int Command_GetAcadia = 1;
        const int Command_GetArches = 2;
        const int Command_GetCuyahoga = 3;
        const int Command_Quit = 0;
        const int Command_ViewCampgrounds = 1;
        const int Command_SearchForReservation = 2;
        const int Command_ReturnToPreviousMenu = 3;
        const int Command_GetSites = 1;
        const int Command_BackToSelectPark = 2;
        private List<int> campgroundList = new List<int>();
        private DateTime arrivalDate;
        private DateTime departureDate;
        private int menuSelection;
        private int parkID;
        private int siteID;
        private string reservationName;
        private Menu menu = new Menu();
        private IParkSqlDAO parkDAO;
        private ICampgroundSqlDAO campgroundDAO;
        private ISiteSqlDAO siteDAO;
        private IReservationSqlDAO reservationDAO;


        public NationalParkCLI(IParkSqlDAO parkDAO, ICampgroundSqlDAO campgroundDAO, ISiteSqlDAO siteDAO, IReservationSqlDAO reservationDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        public void RunCLI()
        {
            
            menu.PrintHeader();

            bool parsedToInt;

                DisplayAvailableParks();
                Console.WriteLine();
                Console.Write("Please choose your option:  ");
            do
            { 
                parsedToInt = int.TryParse(Console.ReadLine().Trim(), out menuSelection);
                if (!parsedToInt)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option.  Please try again.");
                    RunCLI();
                }
                else
                {
                    parkID = menuSelection;
                }

                switch (menuSelection)
                {
                    case Command_GetAcadia:
                        
                        SelectPark(parkID);
                        break;

                    case Command_GetArches:
                        SelectPark(parkID);
                        break;

                    case Command_GetCuyahoga:
                        SelectPark(parkID);
                        break;

                    case Command_Quit:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid option. Please try again.");
                        RunCLI();
                        break;
                        
                }
            } while (!parsedToInt);
        }
 
        private void DisplayAvailableParks()
        {

            IList<Park> parks = parkDAO.DisplayAvailableParks();

            for (int i = 0; i < parks.Count; i++)
            {
                Console.WriteLine($"{i + 1}) Get park information for {parks[i].Name}");
            }
            Console.WriteLine("0) Exit Program");
        }

        private void SelectPark(int userInput)
        {
            bool parsedToInt;
            do
            {
                Console.Clear();                                 
                menu.PrintHeader();
                Park park = parkDAO.SelectPark(parkID);

                Console.WriteLine("Id:".PadRight(14) + $"{park.Id}" + "\nName:".PadRight(15) + $"{park.Name}" + "\nLocation:".PadRight(15) + $"{park.Location}" + "\nEstablished:".PadRight(15) + $"{park.EstablishDate.ToShortDateString()}" + "\nArea:".PadRight(15) + $"{park.Area.ToString("N0")}" + "\nVisitors:".PadRight(15) + $"{park.Visitors.ToString("N0")}" + "\nDescription:".PadRight(15) + $"{park.Description}");
                Console.WriteLine();
                Console.WriteLine($" 1) View campgrounds");
                Console.WriteLine($" 2) Search for reservation");
                Console.WriteLine($" 3) Return to previous screen");
                Console.WriteLine("");
                Console.Write("Please choose your option:  ");
                int menuSelection = 0;
                parsedToInt = int.TryParse(Console.ReadLine().Trim(), out menuSelection);
                if (!parsedToInt)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option.  Please try again.");
                    SelectPark(parkID);
                }
                switch (menuSelection)
                {
                    case Command_ViewCampgrounds:
                        DisplayParkCampgrounds(parkID);
                        Console.WriteLine("Press any key to return to the previous menu.");
                        Console.ReadKey();
                        SelectPark(userInput);
                        break;

                    case Command_SearchForReservation:
                        DisplayParkCampgrounds(parkID);
                        GetAvailableSites();
                        break;

                    case Command_ReturnToPreviousMenu:
                        Console.Clear();
                        RunCLI();
                        break;

                    default:
                        break;
                }
            } while (!parsedToInt);
            
        }
        private void DisplayParkCampgrounds(int command)
        {
            Console.Clear();
            menu.PrintHeader();
            IList<Campground> campgrounds = campgroundDAO.DisplayParkCampgrounds(command);

            Console.WriteLine();
            Console.WriteLine("Id #".PadRight(6) + "Name: ".PadRight(35) + "Open".PadRight(10) + "Close".PadRight(10) + "Daily Fee");
            foreach (Campground campground in campgrounds)
            {
                int openMonth = campground.OpenFromMonth;
                int closeMonth = campground.OpenToMonth;
                DateTimeFormatInfo monthName = new DateTimeFormatInfo();
                campgroundList.Add(campground.Id);
                Console.WriteLine($"{campground.Id}".PadRight(6) + $"{campground.Name}".PadRight(35) + $"{monthName.GetMonthName(openMonth)}".PadRight(10) + $"{monthName.GetMonthName(closeMonth)}".PadRight(10) + $"{campground.DailyFee:C2}");
                Console.WriteLine();
            }
            
        }

        public void GetAvailableSites()
        {
            Console.Write($"Please enter the desired campground ID number (enter 0 to cancel)?:   ");  //TODO Site Id Num
            bool parsedToInt;
                int campgroundSelection;
                parsedToInt = int.TryParse(Console.ReadLine().Trim(), out campgroundSelection);

            if (!parsedToInt || campgroundSelection < 0 || !campgroundList.Contains(campgroundSelection))
            {
                Console.Clear();
                menu.PrintHeader();
                
                DisplayParkCampgrounds(parkID);
                Console.WriteLine("Invalid option. Please try again.");
                GetAvailableSites();
                
            }
            else if (campgroundSelection == 0)
            {
                Console.Clear();
                menu.PrintHeader();
                SelectPark(parkID);
            }
            else
            {
                do
                {
                    Console.Write($"What is the arrival date? (MM/DD/YYYY):   ");
                    arrivalDate = CLIHelper.GetDateTime(Console.ReadLine().Trim());
                    Console.Write($"What is the departure date? (MM/DD/YYYY):  ");
                    departureDate = CLIHelper.GetDateTime(Console.ReadLine().Trim());
                    if( departureDate < arrivalDate)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1 );
                        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                        Console.WriteLine("Departure date must be after arrival date.");
                    }
                    
                } while (departureDate < arrivalDate);

                IList<Site> sites = siteDAO.GetAvailableSites(campgroundSelection, arrivalDate, departureDate);
               
                if (sites.Count == 0)
                {
                    Console.Clear();
                    Console.Write("There are no campsites available for those dates. Would you like to try again (Y/N)?:   ");
                    string tryAgain;// = Console.ReadLine();
                    bool validAnswer = false;
                    do
                    {

                        switch (tryAgain = Console.ReadLine().ToUpper())
                        {
                            case "Y":
                                validAnswer = true;
                                GetAvailableSites();
                                break;
                            case "N":
                                validAnswer = true;
                                RunCLI();
                                break;
                            default:
                                Console.WriteLine("Please answer Y or N.");
                                break;
                        }
                    } while (!validAnswer);
                }
                else
                {
                    Console.Clear();
                    List<int> siteIds = new List<int>();
                    do
                    {
                        menu.PrintHeader();
                        Campground campground = campgroundDAO.SelectCampground(campgroundSelection);
                        Console.WriteLine("Sites are listed in order of popularity.");
                        Console.WriteLine();
                        foreach (Site site in sites)
                        {
                            Console.WriteLine($"Site No.:".PadRight(15) + "Max Occupancy:".PadRight(20) + "Accessible?:".PadRight(15) + "Max RV Length:".PadRight(20) + "Utility:".PadRight(15) + "Daily Fee:");
                            Console.WriteLine($"#{site.Id,-14}{site.MaxOccupancy,-20}{(site.Accessible ? "Yes" : "No"),-15}{(site.MaxRVLength > 0 ? site.MaxRVLength.ToString() : "N/A"),-20}{(site.Utilities ? "Yes" : "No"),-15}{campground.DailyFee:C2}");                                                                                                                                                                                                                       //Pad Right 20 after names
                            Console.WriteLine();
                            siteIds.Add(site.Id);
                        }
                        Console.Write("Which site would you like to reserve (enter 0 to cancel)?:   ");
                        bool couldParse;
                        couldParse = int.TryParse(Console.ReadLine().Trim(), out siteID);

                        if (siteID == 0)
                        {
                            Console.Clear();
                            RunCLI();
                        }
                        else if (!siteIds.Contains(siteID))
                        {
                            Console.WriteLine("Please select a site ID from the list above.");
                        }
                    } while (!siteIds.Contains(siteID));
                        Console.Write("What name should the reservation be made under?:   ");
                        reservationName = Console.ReadLine().Trim();
                }
            }
            AddReservations();
        }
        public void AddReservations()
        {
            Reservation newReservation = new Reservation();
            newReservation.FromDate = arrivalDate;
            newReservation.ToDate = departureDate;
            newReservation.Name = reservationName;
            newReservation.SiteId = Convert.ToInt32(siteID);
            newReservation.CreateDate = DateTime.Now;

            int reservationID = reservationDAO.AddReservations(newReservation);
            Console.WriteLine($"Successful reservation made.  Your reservation ID is {reservationID}");

            Console.WriteLine("Would you like to make another reservation (Y/N)?:   ");
            string keepGoing = Console.ReadLine().ToUpper();
            
            while (true)
            {
                if (keepGoing == "Y" || keepGoing == "N")
                {
                    if (keepGoing.StartsWith("Y"))
                    {
                        RunCLI();
                    }
                    else if (keepGoing.StartsWith("N"))
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Please answer Y or N.");
                    }

                }
            }
        }
    }
}

