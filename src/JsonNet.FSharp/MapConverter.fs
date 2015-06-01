namespace Newtonsoft.Json.FSharp

open System
open System.Collections
open System.Collections.Generic
open Microsoft.FSharp.Reflection
open Newtonsoft.Json
open Newtonsoft.Json.Converters

// Convert to from F# native map type. 
// Code adapted from: http://stackoverflow.com/a/23305471/242348
type MapConverter () =
    inherit JsonConverter ()

    override x.CanConvert(t:Type) =
      t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Map<_, _>>

    override x.WriteJson(writer, value, serializer) =
      serializer.Serialize(writer, value)

    override x.ReadJson(reader, t, _, serializer) =
      let genArgs = t.GetGenericArguments()
      let generify (t:Type) = t.MakeGenericType genArgs
      let tupleType = generify typedefof<Tuple<_, _>>
      let listType = typedefof<List<_>>.MakeGenericType tupleType
      let create (t:Type) types = (t.GetConstructor types).Invoke
      let list = create listType [||] [||] :?> IList
      let kvpType = generify typedefof<KeyValuePair<_, _>>
      for kvp in serializer.Deserialize(reader, generify typedefof<Dictionary<_, _>>) :?> IEnumerable do
        let get name = (kvpType.GetProperty name).GetValue(kvp, null)
        list.Add (create tupleType genArgs [|get "Key"; get "Value"|]) |> ignore        
      create (generify typedefof<Map<_, _>>) [|listType|] [|list|]