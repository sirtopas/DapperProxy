# DapperProxy
A set of extensions to ease the use of Dapper in a .NET project

## Installing
Simply either clone the repo or just copy the DapperProxy.cs and IDapperProxy.cs files into your project. 

## Usage
1. Install Dapper using NuGet
1. Create a BaseRepository:
    ``` c#
    public abstract class BaseRepository
    {
        protected BaseRepository(IDapperProxy dapper)
        {
            Dappter = dapper;
        }

        protected IDapperProxy Dapper { get; }
    }
    ```
1. Use this Base Repo in any other repository:
    ```c#
    public class MyRepository : BaseRepository
    {
        public MyRepository(IDapperProxy dapper) : base(dapper)
        {
        }

        public IEnumerable<MyObject> Search(int objectId)
        {
            var dict = new Dictionary<int, MyObject>();
            var myObjects = Dapper
                                .ClearParameters()
                                .WithStoredProcedure("SearchObjects")
                                .AddParameter("@ObjectId", objectId, DbType.Int32)
                                .Query<MyObject>((myObject) =>
                                    {
                                        if (!dict.TryGetValue(myObject.id, out var item))
                                        {
                                            item = myObject;
                                            dict.Add(item);
                                        }
                                        return item;
                                    },
                                    splitOn: "ObjectId")
                                .Distinct()
                                .ToList();
            return myObjects;
        }
    }