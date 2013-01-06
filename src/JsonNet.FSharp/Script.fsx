// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#I @"C:\Users\Leo\Documents\GitHub\JsonNet.FSharp\src\lib"
#r "Newtonsoft.Json.dll"

#load "OptionConverter.fs"
open Newtonsoft.Json.FSharp

#load "TupleArrayConverter.fs"
open Newtonsoft.Json.FSharp

#load "ListConverter.fs"
open Newtonsoft.Json.FSharp

open System
open Newtonsoft.Json

type HaveAList = {
    Name : string
    Roles : string list
    Prefs : string * Int64
}

let entity = { 
    Name = "Leo G"; 
    Roles = ["admin"];
    Prefs = ("hello",1985L)
}

let converters : JsonConverter[] = [| new ListConverter(); new TupleArrayConverter() |]

let json = JsonConvert.SerializeObject(value=entity, converters=converters)

printfn "The JSON is %A" json

let back = JsonConvert.DeserializeObject<HaveAList>(value=json, converters=converters)

printfn "The entity is %A" back