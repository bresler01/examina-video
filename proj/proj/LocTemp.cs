using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj
{
    internal class LocTemp
    {
        public string UrlVideo;
        private TimeSpan tempo;
        private double x, y;
        public LocTemp()
        {
            UrlVideo = "";
            tempo = TimeSpan.Zero;
            x = 0.0;
            y = 0.0;
        }

        public LocTemp(string urlVideo, TimeSpan tempo, double x, double y)
        {
            this.UrlVideo = urlVideo;
            this.tempo = tempo;
            this.x = x;
            this.y = y;
        }

        public void SetUrl(string UrlVideo)
        {
            this.UrlVideo = UrlVideo;
        }
        public string GetUrl()
        {
            return this.UrlVideo;
        }

        public void SetTempo(TimeSpan tempo)
        {
            this.tempo = tempo;
        }
        public TimeSpan GetTempo()
        {
            return this.tempo;
        }

        public void SetX(double x)
        {
            this.x = x;
        }
        public double GetX()
        {
            return this.x;
        }

        public void SetY(double y)
        {
            this.y = y;
        }
        public double GetY()
        {
            return this.y;
        }

        public override string ToString()
        {
            return $"{UrlVideo};{tempo.TotalSeconds};{x};{y}";
        }

        public static LocTemp FromString(string linha)
        {
            if (string.IsNullOrWhiteSpace(linha))
                return null;

            var partes = linha.Split(';');
            if (partes.Length != 4)
                return null;

            string url = partes[0];

            if (!double.TryParse(partes[1], out double segundos))
                return null;

            if (!double.TryParse(partes[2], out double x))
                return null;

            if (!double.TryParse(partes[3], out double y))
                return null;

            TimeSpan tempo = TimeSpan.FromSeconds(segundos);

            return new LocTemp(url, tempo, x, y);
        }







    }
}
