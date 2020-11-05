using ClaimBuddy.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimBuddy.Repositories
{
    public class MyListRepository : BaseRepository, IMyListRepository
    {
        public MyListRepository(IConfiguration config) : base(config) { }

        public List<MyList> GetAll(int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               Name,
                                               UserProfileId 
                                          FROM List
                                         WHERE UserProfileId = @UserProfileId";
                    cmd.Parameters.AddWithValue("@UserProfileId", userProfileId);
                    var myLists = new List<MyList>();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        myLists.Add(new MyList()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId"))
                        });
                    }
                    reader.Close();
                    return myLists;
                }
            }
        }

        public MyList GetById(int id, int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT l.Id, l.Name, l.UserProfileId,
                                               i.Id AS ItemId, i.Name AS ItemName, i.Model, i.Price, i.CategoryId, i.Notes, i.PurchaseDateTime,
                                               c.Name as CategoryName
                                          FROM List l
                                     LEFT JOIN ListItem li ON l.Id = li.ListId
                                     LEFT JOIN Item i ON li.ItemId = i.Id
                                     LEFT JOIN Category c on i.CategoryId = c.Id
                                         WHERE l.Id = @Id AND l.UserProfileId = @UserProfileId"; 
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@UserProfileId", userProfileId);
                    var reader = cmd.ExecuteReader();
                    MyList myList = null;
                    while (reader.Read())
                    {
                        if (myList == null)
                        {
                            myList = new MyList()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                                Items = new List<Item>()
                            };
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("ItemId")))
                        {
                            Item item = new Item()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ItemId")),
                                Name = reader.GetString(reader.GetOrdinal("ItemName")),
                                PurchaseDateTime = reader.GetDateTime(reader.GetOrdinal("PurchaseDateTime")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                Category = new Category()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                                }
                            };
                            myList.Items.Add(item);
                        }
                    }
                    reader.Close();
                    return myList;
                }
            }
        }

        public void Add(MyList myList)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO List (Name, UserProfileId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@Name, @UserProfileId)";
                    cmd.Parameters.AddWithValue("@Name", myList.Name);
                    cmd.Parameters.AddWithValue("@UserProfileId", myList.UserProfileId);
                    myList.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Delete(int myListId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM ListItem WHERE ListId = @ListId;
                                        DELETE FROM List WHERE Id = @ListId;";
                    cmd.Parameters.AddWithValue("@ListId", myListId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(MyList myList, List<int> selectedItemIds)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE List SET Name = @Name WHERE Id = @ListId;
                                        DELETE FROM ListItem WHERE ListId = @ListId";
                    cmd.Parameters.AddWithValue("@ListId", myList.Id);
                    cmd.Parameters.AddWithValue("@Name", myList.Name);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"INSERT INTO ListItem (ListId, ItemId) 
                                        VALUES (@ListId, @ItemId)";
                    foreach (int itemId in selectedItemIds)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ListId", myList.Id);
                        cmd.Parameters.AddWithValue("@ItemId", itemId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
