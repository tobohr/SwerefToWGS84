using MightyLittleGeodesy.Positions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SwerefToWGS84 {

    class Program {

        static void Main(string[] args) {
            Console.WriteLine("Write the name of the file with sweref coordinates, each coordinate seperated by ; see example.txt");
            string file = Console.ReadLine();
            string line;
            var pos = new List<WGS84Position>();
            var XystringWithLinebreak = new List<string>();
            var Xystring = new List<string>();
            try {
                using (StreamReader sr = new StreamReader(file)) {
                    line = sr.ReadToEnd();
                    XystringWithLinebreak = line.Split(';').ToList();
                    for (var i = 0; i < XystringWithLinebreak.Count; i++) {
                        if (i == 0) {
                            Xystring.Add(XystringWithLinebreak.ElementAt(i));
                        }
                        else if (i == XystringWithLinebreak.Count - 1) {
                            Xystring.Add(XystringWithLinebreak.ElementAt(i).Split('\r').First());
                        }
                        else { 
                            Xystring.Add(XystringWithLinebreak.ElementAt(i).Split('\r').First());
                            Xystring.Add(XystringWithLinebreak.ElementAt(i).Split('\n').ElementAt(1));
                        }
                    }
                }
                for (var i = 0; i < Xystring.Count; i += 2) {
                    SWEREF99Position swePos = new SWEREF99Position(float.Parse(Xystring.ElementAt(i)), float.Parse(Xystring.ElementAt(i + 1)));
                    pos.Add(swePos.ToWGS84());
                    Console.Write("Lat: " + swePos.ToWGS84().Latitude.ToString() + " Long: " + swePos.ToWGS84().Longitude.ToString() + "\n");
                }

            } catch (Exception e) {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\nEnter filename for output file");
            string nameoutput = Console.ReadLine();
            using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(nameoutput)) {
                foreach (var item in pos) {
                    file2.WriteLine(item.Latitude.ToString() + ";" + item.Longitude.ToString());
                }
            }
        }
    }
}