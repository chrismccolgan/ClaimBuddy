﻿using System.Collections.Generic;
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
                return @"SELECT i.Id, i.Name, i.Notes, i.Price, i.PurchaseDateTime, i.CreateDateTime, i.UserProfileId, i.CategoryId, i.Image, i.ReceiptImage, i.Model,
                                c.Name AS CategoryName
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
                ReceiptImage = reader.IsDBNull(reader.GetOrdinal("ReceiptImage")) ? null : reader.GetString(reader.GetOrdinal("ReceiptImage")),
                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                PurchaseDateTime = reader.GetDateTime(reader.GetOrdinal("PurchaseDateTime")),
                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                Category = new Category()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                }
            };
        }

        public List<Item> GetAll(int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = ItemQuery + " AND i.UserProfileId = @UserProfileId ORDER BY i.Name";
                    cmd.Parameters.AddWithValue("@UserProfileId", userProfileId);
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

        public Item GetById(int id, int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = ItemQuery + " AND i.Id = @Id AND i.UserProfileId = @UserProfileId";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@UserProfileId", userProfileId);
                    var reader = cmd.ExecuteReader();
                    Item item = null;
                    if (reader.Read())
                    {
                        item = NewItem(reader);
                    }
                    reader.Close();
                    return item;
                }            
            }
        }

        public void Add(Item item)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Item (Name, Notes, Price, Image, PurchaseDateTime, CreateDateTime, IsDeleted, CategoryId, UserProfileId, Model, ReceiptImage)
                                        OUTPUT INSERTED.ID
                                        VALUES (@Name, @Notes, @Price, @Image, @PurchaseDateTime, @CreateDateTime, @IsDeleted, @CategoryId, @UserProfileId, @Model, @ReceiptImage)";
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Notes", item.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Image", item.Image ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReceiptImage", item.ReceiptImage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Model", item.Model ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PurchaseDateTime", item.PurchaseDateTime);
                    cmd.Parameters.AddWithValue("@CreateDateTime", item.CreateDateTime);
                    cmd.Parameters.AddWithValue("@IsDeleted", item.IsDeleted);
                    cmd.Parameters.AddWithValue("@CategoryId", item.CategoryId);
                    cmd.Parameters.AddWithValue("@UserProfileId", item.UserProfileId);
                    item.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Item item)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Item
                                           SET Name = @Name,
                                               Notes = @Notes,
                                               Model = @Model,
                                               Price = @Price, 
                                               Image = @Image, 
                                               ReceiptImage = @ReceiptImage, 
                                               PurchaseDateTime = @PurchaseDateTime,
                                               IsDeleted = @IsDeleted,
                                               CategoryId = @CategoryId
                                         WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Notes", item.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Image", item.Image ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReceiptImage", item.ReceiptImage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Model", item.Model ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PurchaseDateTime", item.PurchaseDateTime);
                    cmd.Parameters.AddWithValue("@IsDeleted", item.IsDeleted);
                    cmd.Parameters.AddWithValue("@CategoryId", item.CategoryId);
                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Item 
                                           SET IsDeleted = 1 
                                         WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
