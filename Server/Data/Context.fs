namespace Server.Data

open System.Threading.Tasks
open Dapper.FSharp.MySQL
open Microsoft.Extensions.Configuration
open MySql.Data.MySqlClient
open Server.Config
open System.Data
open Shared.Dtos


type IContext<'a> =
    abstract All: unit -> Task<'a seq>
    abstract Where: predicate: ('a -> bool) -> Task<'a seq>
    abstract Update: entry: 'a -> Task<int>

type BlogContext(config: IConfiguration) =
    let connectionString = config.Get<Config>().Database.ConnectionString
    let connection: IDbConnection = new MySqlConnection(connectionString)
    let table = table'<BlogEntry> Tables.BlogEntries.name

    interface IContext<BlogEntry> with
        member this.All() =
            select {
                for entry in table do
                selectAll
            }
            |> connection.SelectAsync<BlogEntry>

        member this.Where predicate =
            select {
                for entry in table do
                where (predicate entry)
            }
            |> connection.SelectAsync<BlogEntry>
        
        member this.Update entry =
            update {
                for e in table do
                set entry
                where (e.Slug = entry.Slug)
            }
            |> connection.UpdateAsync

type SongContext(config: IConfiguration) =
    let connectionString = config.Get<Config>().Database.ConnectionString
    let connection: IDbConnection = new MySqlConnection(connectionString)
    let table = table'<Song> Tables.Songs.name

    interface IContext<Song> with
        member this.All() =
            select {
                for entry in table do
                    selectAll
            }
            |> connection.SelectAsync<Song>

        member this.Where predicate =
            select {
                for entry in table do
                    where (predicate entry)
            }
            |> connection.SelectAsync<Song  >

        member this.Update entry =
            update {
                for e in table do
                    set entry
                    where (e.Slug = entry.Slug)
            }
            |> connection.UpdateAsync
