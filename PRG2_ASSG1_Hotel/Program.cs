// Zhi Wei does 2,4,6
// Jia Xian does 1,3,5
using PRG2_ASSG1_Hotel;

List<Guest> guestList = new List<Guest>();
//Initialise Data from csv files
void InitData()
{
    List<List<string>> roomlist = new List<List<string>>();
    List<List<string>> guestlist = new List<List<string>>(); // init string-list var
    List<List<string>> stayslist = new List<List<string>>();
    //Reading data from csv
    string[] roomdata = File.ReadAllLines("Rooms.csv");
    string[] guestdata = File.ReadAllLines("Guests.csv"); // readalllines of CSV
    string[] staysdata = File.ReadAllLines("Stays.csv");
    for (int i = 1; i < roomdata.Length; i++)
    {
        List<string> values = roomdata[i].Split(',').ToList();
        //Adding data into list
        roomlist.Add(values);
    }
    for (int i = 1; i < guestdata.Length; i++)
    {
        List<string> values = guestdata[i].Split(',').ToList();
        //Adding data into list
        guestlist.Add(values);
    }
    for (int i = 1; i < staysdata.Length; i++)
    {
        List<string> values = staysdata[i].Split(',').ToList();
        //Adding data into list
        stayslist.Add(values);
    }
    foreach (List<string> guest in guestlist)
    {
        Membership membership = new Membership();
        Stay stay = new Stay();
        guestList.Add(new Guest(guest[0], guest[1], stay, membership));
        foreach (List<string> bookings in stayslist) // iterate through bookinglist
        {
            Stay book = new Stay(Convert.ToDateTime(bookings[3]), Convert.ToDateTime(bookings[4]));
            foreach (List<string> rooms in roomlist) // iterate through roomslist
            {
                if (rooms[1] == bookings[5]) // if roomno matches
                {
                    if (rooms[0] == "Standard") // if type is standard
                    {
                        StandardRoom room = new StandardRoom(Convert.ToInt32(rooms[1]), rooms[2], Convert.ToDouble(rooms[3]), !(Convert.ToBoolean(bookings[2])));
                        room.RequireWifi = Convert.ToBoolean(bookings[6]);
                        room.RequireBreakfast = Convert.ToBoolean(bookings[7]);
                        book.AddRoom(room);

                    }
                    else if (rooms[0] == "Deluxe") // if type is deluxe
                    {
                        DeluxeRoom room = new DeluxeRoom(Convert.ToInt32(rooms[1]), rooms[2], Convert.ToDouble(rooms[3]), !(Convert.ToBoolean(bookings[2])));
                        room.AdditionalBed = Convert.ToBoolean(bookings[12]);
                        book.AddRoom(room);
                    }
                    foreach (List<string> guestprof in guestlist)
                    {
                        if (guest[1] == bookings[1])
                        {
                            Membership m = new Membership(guestprof[2], Convert.ToInt32(guestprof[3]));
                            Guest guestobj = new Guest(guestprof[0], guestprof[1], book, m);
                            guestobj.IsCheckedin = Convert.ToBoolean(bookings[2]);
                            guestList.Add(guestobj);
                        }
                    }
                }
            }
        }

    }
}
void DisplayGuest(List<Guest> guestList)
{
    foreach (Guest g in guestList)
    {
        Console.WriteLine(g.ToString());
    }
}

void DisplayRoom(List<Room> roomList)
{
    foreach (Room r in roomList)
    {
        Console.WriteLine(r.ToString());
    }
}

void RegisterGuest()
{
    string gName;
    string gPN;
    Console.WriteLine("Please enter the following information\n");
    Console.Write("Guest name: ");
    gName = Console.ReadLine();
    Console.Write("Guest passport number: ");
    gPN = Console.ReadLine();
    Stay stay = new Stay();
    Membership membership = new Membership();
    Guest guest = new Guest(gName, gPN, stay, membership);
}

int entOpt;
InitData();
while (true)
{
    try
    {
        Console.WriteLine("\n------ Hotel Guest Management System ------");
        Console.WriteLine("[1]. Display all guests\n[2]. Display all available rooms\n[3]. Register guest\n[4]. Check-in guest\n[5]. Display all details for guest\n[6]. Extend days for stay\n[0]. Quit Hotal Guest Management System");
        Console.Write("-------------------------------------------\nPlease enter your option: ");
        entOpt = Convert.ToInt32(Console.ReadLine());
        if (entOpt == 0)
        {
            break;
        }
        else if (entOpt == 1)
        {
            Console.WriteLine("\n------ Displaying all guests ------\n");
            DisplayGuest(guestList);
        }
        else if (entOpt == 2)
        {
            Console.WriteLine("\n------ Displaying all available rooms ------\n");
        }
        else if (entOpt == 3)
        {
            Console.WriteLine("\n------------- Register Guest --------------\n");
            RegisterGuest();
        }
        else if (entOpt == 4)
        {
            Console.WriteLine("\n------ Guest Check-IN ------\n");
        }
        else if (entOpt == 5)
        {
            Console.WriteLine("\n------ Displaying all details for Guest ------\n");
        }
        else if (entOpt == 6)
        {
            Console.WriteLine("\n------ Extending days for stay ------\n");
        }
        else
        {
            Console.WriteLine("\nPlease enter a numeric value from 0 - 6");
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"\nIncorrect values! {ex.Message} Please try again with numeric values from 0 - 6");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nPlease enter the correct value format and try again. The option only accept numeric values from 0 - 6");
    }
}





