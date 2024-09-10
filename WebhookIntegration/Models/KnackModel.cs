public class Record
{
    public string Id { get; set; }
    public int Field605 { get; set; }
    public int Field605Raw { get; set; }
    public string Field566 { get; set; }
    public List<Field566Raw> Field566Raw { get; set; }
    public string Field564 { get; set; }
    public string Field564Raw { get; set; }
    public string Field565 { get; set; }
    public Field565Raw Field565Raw { get; set; }
    public string Field730 { get; set; }
    public Field730Raw Field730Raw { get; set; }
    public string Field563 { get; set; }
    public string Field563Raw { get; set; }
    public string Field875 { get; set; }
    public string Field875Raw { get; set; }
    public string Field876 { get; set; }
    public string Field876Raw { get; set; }
    public string Field879 { get; set; }
    public bool Field879Raw { get; set; }
}

public class Field566Raw
{
    public string Id { get; set; }
    public string Identifier { get; set; }
}

public class Field565Raw
{
    public string Date { get; set; }
    public string DateFormatted { get; set; }
    public string Hours { get; set; }
    public string Minutes { get; set; }
    public string AmPm { get; set; }
    public long UnixTimestamp { get; set; }
    public DateTime IsoTimestamp { get; set; }
    public string Timestamp { get; set; }
    public int Time { get; set; }
}

public class Field730Raw
{
    public string Date { get; set; }
    public string DateFormatted { get; set; }
    public string Hours { get; set; }
    public string Minutes { get; set; }
    public string AmPm { get; set; }
    public long UnixTimestamp { get; set; }
    public DateTime IsoTimestamp { get; set; }
    public string Timestamp { get; set; }
    public int Time { get; set; }
}

public class KnackModel
{
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int TotalRecords { get; set; }
    public List<Record> Records { get; set; }
}
