using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Contact
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;
}
