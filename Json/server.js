const http = require('http');
const { MongoClient } = require('mongodb');

// ---------------- Mongo Setup ----------------
const uri = process.env.MONGODB_URI;
const client = new MongoClient(uri);
let db;

async function connectDB() {
    if (!db) {
        await client.connect();
        // แก้ชื่อเป็น testdb ให้ตรงกับใน Mongo Compass ของคุณ
        db = client.db('testdb'); 
        console.log("✅ MongoDB connected to testdb");
    }
    return db;
}

// ---------------------------------------------------------------
const PORT = process.env.PORT || 9888;
// ---------------------------------------------------------------

async function onClientRequest(req, resp) {
    const parsedUrl = new URL(req.url, `http://${req.headers.host}`);
    const pathname = parsedUrl.pathname;

    // ตั้งค่า Header ให้ส่งเป็น JSON และรองรับภาษาไทย
    resp.writeHead(200, { 
        'Content-Type': 'application/json; charset=utf-8',
        'Access-Control-Allow-Origin': '*' 
    });

    try {
        const database = await connectDB();

        // 1. API สำหรับดึงรายชื่อแมวทั้งหมด
        if (req.method === 'GET' && pathname === '/api/characters') {
            const cats = await database.collection('characters').find({}).toArray();
            resp.write(JSON.stringify(cats));
        }

        // 2. API สำหรับดึงค่าเงิน
        else if (req.method === 'GET' && pathname === '/api/currency') {
            const currency = await database.collection('currency').findOne({});
            resp.write(JSON.stringify(currency || { message: "No currency data" }));
        }

        // 3. API สำหรับดึงข้อมูล Gacha Config
        else if (req.method === 'GET' && pathname === '/api/gacha') {
            const gacha = await database.collection('gacha').find({}).toArray();
            resp.write(JSON.stringify(gacha));
        }

        // 4. API Weapon Stat เดิม (เผื่อยังใช้งาน)
        else if (req.method === 'GET' && pathname === '/api/weapon_stat') {
            const item_id = parsedUrl.searchParams.get('item_id');
            const weapon = await database.collection('weapons').findOne({
                $or: [{ item_id: Number(item_id) }, { item_id: String(item_id) }]
            });
            resp.write(JSON.stringify(weapon || { message: "Weapon not found" }));
        }

        // Default ถ้าไม่ตรงกับ Path ไหนเลย
        else {
            resp.write(JSON.stringify({ 
                message: 'API Online', 
                available_endpoints: ['/api/characters', '/api/currency', '/api/gacha'] 
            }));
        }
    } catch (err) {
        console.error(err);
        resp.write(JSON.stringify({ error: "Database error", details: err.message }));
    }

    resp.end();
}

const server = http.createServer(onClientRequest);
server.listen(PORT, () => {
    console.log('Server is running on port ' + PORT);
});