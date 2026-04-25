import { MongoClient } from "mongodb";
const uri = process.env.MONGODB_URI;
let clientPromise = new MongoClient(uri).connect();

export default async function handler(req, res) {
  try {
    const client = await clientPromise;
    const db = client.db("testdb");
    // ดึงข้อมูลเงินทั้งหมด (สมมติว่าเก็บไว้ใน document เดียว)
    const data = await db.collection("currencies").findOne({});
    res.status(200).json(data || { coin: 0, diamond: 0 });
  } catch (e) { res.status(500).json({ error: e.message }); }
}