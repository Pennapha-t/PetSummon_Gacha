const http = require('http')
const { MongoClient } = require('mongodb')
const api_gacha = require('./api/gacha')

// ---------------- Mongo Setup ----------------
const uri = process.env.MONGODB_URI

if (!uri) {
    console.error("❌ MONGODB_URI not found")
}

const client = new MongoClient(uri)
let db

async function connectDB() {
    if (!db) {
        await client.connect()
        db = client.db('game_db')
        console.log("✅ MongoDB connected")
    }
    return db
}
// --------------------------------------------

// ---------------------------------------------------------------
const PORT = process.env.PORT || 9888
// ---------------------------------------------------------------

async function onClientRequest(req, resp)
{
    const pathname = req.url.split('?')[0]

    resp.writeHead(200, { 'Content-Type': 'application/json' })

    // ✅ GET weapon (ใช้ MongoDB)
    if (req.method === 'GET' && pathname === '/api/weapon_stat') {

        const item_id = new URL(req.url, `http://mongodb+srv://<pastelskyk_db_user>:<rzNtaHidOJxpiFVm>@cluster0.r8ae8az.mongodb.net/`)
            .searchParams.get('item_id')

        try {
            const database = await connectDB()

            const weapon = await database.collection('weapons').findOne({
                item_id: parseInt(item_id)
            })

            if (!weapon) {
                resp.write(JSON.stringify({ message: "Weapon not found" }))
            } else {
                resp.write(JSON.stringify(weapon))
            }

        } catch (err) {
            console.error(err)
            resp.write(JSON.stringify({ error: "Database error" }))
        }
    }

    // ✅ POST monster (ของเดิม)
    else if (req.method === 'POST' && pathname === '/api/get_monsters') {
        api_gacha.onRequestGetMonsters(req, resp)
        return
    }

    // ✅ default
    else {
        resp.write(JSON.stringify({ message: 'Hello Vercel' }))
    }

    resp.end()
}

// ---------------------------------------------------------------
const server = http.createServer(onClientRequest)
server.listen(PORT)

console.log('running on ' + PORT)