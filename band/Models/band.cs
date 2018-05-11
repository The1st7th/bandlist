using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandList;
using System;

namespace BandList.Models
{
    public class Band
    {
        private string _bandname;
        private int _id;
        // We no longer declare _categoryId here

        public Band(string bandname, int id = 0)
        {
            _bandname = bandname;
            _id = id;
           // categoryId is removed from the constructor
        }

        public override bool Equals(System.Object otherItem)
        {
          if (!(otherItem is Band))
          {
            return false;
          }
          else
          {
             Band newItem = (Band) otherItem;
             bool idEquality = this.GetId() == newItem.GetId();
             bool descriptionEquality = this.GetBandname() == newItem.GetBandname();
             // We no longer compare Items' categoryIds in a categoryEquality bool here.
             return (idEquality && descriptionEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetBandname().GetHashCode();
        }

        public string GetBandname()
        {
            return _bandname;
        }

        public int GetId()
        {
            return _id;
        }

        // We've removed the GetCategoryId() method entirely.

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO bands (bandname) VALUES (@bandname);";

            MySqlParameter bandname = new MySqlParameter();
            bandname.ParameterName = "@bandname";
            bandname.Value = this._bandname;
            cmd.Parameters.Add(bandname);

            // Code to declare, set, and add values to a categoryId SQL parameters has also been removed.

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Band> GetAll()
        {
            List<Band> allItems = new List<Band> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM bands;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int bandId = rdr.GetInt32(0);
              string bandname = rdr.GetString(1);
              // We no longer need to read categoryIds from our items table here.
              // Constructor below no longer includes a itemCategoryId parameter:
              Band newItem = new Band(bandname, bandId);
              allItems.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }

        public static Band Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int bandId = 0;
            string bandname = "";
            // We remove the line setting a itemCategoryId value here.

            while(rdr.Read())
            {
              bandId = rdr.GetInt32(0);
              bandname = rdr.GetString(1);
              // We no longer read the itemCategoryId here, either.
            }

            // Constructor below no longer includes a itemCategoryId parameter:
            Band newItem = new Band(bandname, bandId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newItem;
        }

        public void UpdateDescription(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE bands SET bandname = @newDescription WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter bandname = new MySqlParameter();
            bandname.ParameterName = "@newDescription";
            bandname.Value = newDescription;
            cmd.Parameters.Add(bandname);

            cmd.ExecuteNonQuery();
            _bandname = newDescription;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }

        public static void Delete(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM bands WHERE id = @searchId; DELETE FROM bands_venues WHERE band_id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM bands; DELETE FROM bands_venues";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void AddVenue(Venue newVenue)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO bands_venues (venue_id, band_id) VALUES (@VenueId, @BandId);";

          MySqlParameter venue_id = new MySqlParameter();
          venue_id.ParameterName = "@venueId";
          venue_id.Value = newVenue.GetId();
          cmd.Parameters.Add(venue_id);

          MySqlParameter band_id = new MySqlParameter();
          band_id.ParameterName = "@BandId";
          band_id.Value = _id;
          cmd.Parameters.Add(band_id);

          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
        }
        public List<Venue> GetVenues()
         {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"SELECT Venue_id FROM Bands_Venues WHERE band_id = @bandId;";

           MySqlParameter itemIdParameter = new MySqlParameter();
           itemIdParameter.ParameterName = "@bandId";
           itemIdParameter.Value = _id;
           cmd.Parameters.Add(itemIdParameter);

           var rdr = cmd.ExecuteReader() as MySqlDataReader;

           List<int> categoryIds = new List<int> {};
           while(rdr.Read())
           {
               int categoryId = rdr.GetInt32(0);
               categoryIds.Add(categoryId);
           }
           rdr.Dispose();

           List<Venue> categories = new List<Venue> {};
           foreach (int categoryId in categoryIds)
           {
               var categoryQuery = conn.CreateCommand() as MySqlCommand;
               categoryQuery.CommandText = @"SELECT * FROM venues WHERE id = @VenueId;";

               MySqlParameter VenueIdParameter = new MySqlParameter();
               VenueIdParameter.ParameterName = "@VenueId";
               VenueIdParameter.Value = categoryId;
               categoryQuery.Parameters.Add(VenueIdParameter);

               var categoryQueryRdr = categoryQuery.ExecuteReader() as MySqlDataReader;
               while(categoryQueryRdr.Read())
               {
                   int thisCategoryId = categoryQueryRdr.GetInt32(0);
                   string categoryName = categoryQueryRdr.GetString(1);
                   Venue foundCategory = new Venue(categoryName, thisCategoryId);
                   categories.Add(foundCategory);
               }
               categoryQueryRdr.Dispose();
           }
           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
           return categories;
         }

    }
}
