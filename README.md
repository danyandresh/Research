# yellow

start ui server: npm run ng serve -- --host=0.0.0.0

db.createUser(
  {
    user: "yellow",
    pwd: "abcsda",
    roles: [ { role: "userAdminAnyDatabase", db: "admin" } ]
  }
)

db.createUser(
  {
    user: "yell",
    pwd: "abcsda",
    roles: [ { role: "readWrite", db: "local" } ]
  }
)

mongodb://yell:abcsda@52.169.255.164:27017/local?authSource=admin


"C:\Program Files\MongoDB\Server\3.2.22\bin\mongod.cfg"
