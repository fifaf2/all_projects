using System;
using System.Collections.Generic;
using System.Text;
using WPFekz.Models;

namespace WPFekz
{
    public static class Appconnect
    {
        public static ZxcContext db = new ZxcContext();
        public static User? user = null;
    }
}