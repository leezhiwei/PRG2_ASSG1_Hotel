// Zhi Wei does 2,4,6
// Jia Xian does 1,3,5
using PRG2_ASSG1_Hotel;

List<Guest> guestList = new List<Guest>();
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
                foreach (List<string> guest in guestlist)
                {
                    if (guest[1] == bookings[1])
                    {
                        Membership m = new Membership(guest[2], Convert.ToInt32(guest[3]));
                        Guest guestobj = new Guest(guest[0], guest[1], book, m);
                        guestobj.IsCheckedin = Convert.ToBoolean(bookings[2]);
                        guestList.Add(guestobj);
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


InitData();
DisplayGuest(guestList);