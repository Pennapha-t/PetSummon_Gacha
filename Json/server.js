const http = require('http')
const api_gacha = require('./api/gacha')

// ---------------------------------------------------------------
const PORT = process.env.PORT || 9888
// ---------------------------------------------------------------

function onClientRequest(req, resp)
{
    const pathname = req.url.split('?')[0]

    resp.writeHead(200, { 'Content-Type': 'application/json' })

    // ✅ GET weapon
    if (req.method === 'GET' && pathname === '/api/weapon_stat') {

        const item_id = new URL(req.url, `http://${req.headers.host}`)
            .searchParams.get('item_id')

        const data = {
            item_id: parseInt(item_id),
            name: "Wood Sword",
            damage: 85,
            sharpness: 10
        }

        resp.write(JSON.stringify(data))
    }

    // ✅ POST monster (ของเดิมอาจารย์)
    else if (req.method === 'POST' && pathname === '/api/get_monsters') {
        api_gacha.onRequestGetMonsters(req, resp)
        return // ❗ สำคัญ (ให้ api จัดการ resp เอง)
    }

    // ✅ default
    else {
        resp.write(JSON.stringify({ message: 'Hello Vercel' }))
    }

    resp.end() // ❗ ต้องมี
}

// ---------------------------------------------------------------
const server = http.createServer(onClientRequest)
server.listen(PORT)

console.log('running on ' + PORT)