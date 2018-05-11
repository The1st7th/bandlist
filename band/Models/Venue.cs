using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandList.Models
{
    public class Venue
    {
        private string _venuename;
        private int _id;

        public Venue(string venuename, int id = 0)
        {
            _venuename = venuename;
            _id = id;
        }
        public override bool Equals(System.Object otherVenue)
        {
            if (!(otherVenue is Venue))
            {
                return false;
            }
            else
            {
                Venue newCategory = (Venue) otherVenue;
                return this.GetId().Equals(newCategory.GetId());
            }
        }
        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }
        public string GetVenuename()
        {
            return _venuename;
        }
        public int GetId()
        {
            return _id;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO venues (venuename) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._venuename;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }
        public static List<Venue> GetAll()
        {
            List<Venue> allCategories = new List<Venue> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM venues;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int VenueId = rdr.GetInt32(0);
              string VenueName = rdr.GetString(1);
              Venue newCategory = new Venue(VenueName, VenueId);
              allCategories.Add(newCategory);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCategories;
        }
        public static Venue Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int VenueId = 0;
            string VenueName = "";

            while(rdr.Read())
            {
              VenueId = rdr.GetInt32(0);
              VenueName = rdr.GetString(1);
            }
            Venue newCategory = new Venue(VenueName, VenueId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newCategory;
        }
        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM venues;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public List<Band> GetBands()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT bands.* FROM venues
                JOIN bands_venues ON (venues.id = bands_venues.venue_id)
                JOIN bands ON (bands_venues.band_id = bands.id)
                WHERE venues.id = @VenueId;";

            MySqlParameter venueIdParameter = new MySqlParameter();
            venueIdParameter.ParameterName = "@VenueId";
            venueIdParameter.Value = _id;
            cmd.Parameters.Add(venueIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Band> bands = new List<Band>{};

            while(rdr.Read())
            {
              int bandId = rdr.GetInt32(0);
              string bandName = rdr.GetString(1);
              Band newItem = new Band(bandName, bandId);
              bands.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return bands;
        }
    }
}
