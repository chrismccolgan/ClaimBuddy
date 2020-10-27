using Microsoft.Data.SqlClient;
using ClaimBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ClaimBuddy.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public UserProfile GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Email, FirebaseUserId, FirstName, LastName
                                          FROM UserProfile
                                         WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@id", id);
                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName"))
                        };
                    }
                    reader.Close();
                    return userProfile;
                }
            }
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Email, FirebaseUserId, FirstName, LastName
                                          FROM UserProfile
                                         WHERE FirebaseUserId = @FirebaseUserId";
                    cmd.Parameters.AddWithValue("@FirebaseUserId", firebaseUserId);
                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName"))
                        };
                    }
                    reader.Close();
                    return userProfile;
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProfile (Email, FirebaseUserId, FirstName, LastName, CreateDateTime) 
                                        OUTPUT INSERTED.ID
                                        VALUES (@Email, @FirebaseUserId, @FirstName, @LastName, @CreateDateTime)";
                    cmd.Parameters.AddWithValue("@Email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@FirebaseUserId", userProfile.FirebaseUserId);
                    cmd.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@CreateDateTime", userProfile.CreateDateTime);
                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}