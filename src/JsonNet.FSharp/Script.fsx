#I @"..\lib"
#r "Newtonsoft.Json.dll"

#load "OptionConverter.fs"

#load "TupleArrayConverter.fs"

#load "ListConverter.fs"

#load "SingleCaseUnionConverter.fs"

#load "UnionEnumConverter.fs"

open Newtonsoft.Json.FSharp

open System
open Newtonsoft.Json

type Email = Email of string

type Role = Admin | Sysop | User

type Employee = {
    Name : string
    Roles : Role list
    Location : string * Int32
    Email : Email
}

let entity = { 
    Name = "Leo G"; 
    Roles = [Admin; User];
    Location = ("Complex 4",4)
    Email = Email "leo.g@corporation.com"
}

let converters : JsonConverter[] = [| new ListConverter(); new TupleArrayConverter(); new SingleCaseUnionConverter(); new UnionEnumConverter()|]

let json = JsonConvert.SerializeObject(value=entity, converters=converters)

assert (json = """{"Name":"Leo G","Roles":["Admin","User"],"Location":["Complex 4",4],"Email":"leo.g@corporation.com"}""")

let back = JsonConvert.DeserializeObject<Employee>(value=json, converters=converters)

assert (back = entity)