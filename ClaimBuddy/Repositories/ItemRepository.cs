using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ClaimBuddy.Models;
using System;

namespace ClaimBuddy.Repositories
{
    public class ItemRepository : BaseRepository, IItemRepository
    {
        public ItemRepository(IConfiguration configuration) : base(configuration) { }

        private string ItemQuery
        {
            get
            {
                return @"SELECT i.Id, i.Name, i.Notes, i.Price, i.PurchaseDate, i.CreateDateTime, i.UserProfileId, i.CategoryId, i.Image
                           FROM Item i
                      LEFT JOIN UserProfile up on i.UserProfileId = up.Id
                      LEFT JOIN Category c on i.CategoryId = c.Id
                          WHERE i.IsDeleted = 0";
            }
        }

        private Item NewItem(SqlDataReader reader)
        {
            return new Item()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
            };
        }

        public List<Item> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = ItemQuery;
                    var items = new List<Item>();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        items.Add(NewItem(reader));
                    }
                    reader.Close();
                    return items;
                }
            }
        }

        //public object GetbyId(int id)
        //{
        //    using (var conn = Connection)
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = QuoteQuery + " WHERE q.id = @Id";
        //            DbUtils.AddParameter(cmd, "@Id", id);

        //            Quote quote = null;

        //            var reader = cmd.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                quote = NewQuote(reader);
        //            }
        //            reader.Close();

        //            return quote;
        //        }
        //    }
        //}

        //public void Add(Quote quote)
        //{
        //    using (var conn = Connection)
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"INSERT INTO Quote (Text, UserProfileId)
        //                                OUTPUT INSERTED.ID
        //                                VALUES (@Text, @UserProfileId)";
        //            DbUtils.AddParameter(cmd, "@Text", quote.Text);
        //            DbUtils.AddParameter(cmd, "@UserProfileId", quote.UserProfileId);

        //            quote.Id = (int)cmd.ExecuteScalar();
        //        }
        //    }
        //}

    }
}
