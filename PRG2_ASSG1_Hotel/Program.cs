// Zhi Wei does 2,4,6
// Jia Xian does 1,3,5
using PRG2_ASSG1_Hotel;

List<Guest> guestList = new List<Guest>();

void InitGuest(List<Guest> guestList)
{
    //Reading data from csv
    string[] csvLines = File.ReadAllLines("Guests.csv");
    for (int i = 1; i < csvLines.Length; i++)
    {
        string[] values = csvLines[i].Split(',');
        //Adding data into list
        guestList.Add(new Guest(values[0], values[1], values[2], values[3]));
    }
}

void DisplayGuest(List<Guest> guestList)
{
    foreach (Guest g in guestList)
    {
        Console.Writeline(g.ToString());    
    }
}


InitGuest(guestList);
DisplayGuest(guestList);

