struct CityNameLinkPair
{
    public string Name { get; set; } = "";
    public string Link { get; set; } = "";
    public CityNameLinkPair(string name, string link)
    {
        Name = name;
        Link = link;
    }
}
