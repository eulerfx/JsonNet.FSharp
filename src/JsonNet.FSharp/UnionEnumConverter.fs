namespace Newtonsoft.Json.FSharp

open System
open Microsoft.FSharp.Reflection
open Newtonsoft.Json
open Newtonsoft.Json.Converters

// Convert unions of form "type Union = A | B | C" to/from json strings
type UnionEnumConverter () =
    inherit JsonConverter ()

    override this.CanConvert(t) =
        FSharpType.IsUnion(t) &&
        not (FSharpType.GetUnionCases(t) |> Array.exists (fun case -> case.GetFields().Length > 0))

    override this.WriteJson(writer, value, serializer) =
        let name =
            if value = null then null
            else
                match FSharpValue.GetUnionFields(value, value.GetType()) with
                | case, _ -> case.Name  
        serializer.Serialize(writer,name)

    override this.ReadJson(reader, t, existingValue, serializer) =
        let value = serializer.Deserialize(reader,typeof<string>) :?> string

        let case = FSharpType.GetUnionCases(t) |> Array.pick (fun case ->
            // Note: Case insensitive match!
            if case.Name.ToUpper() = value.ToUpper() then Some case else None
        )

        FSharpValue.MakeUnion(case,[||])