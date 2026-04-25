import { MongoClient } from "mongodb";

const uri = process.env.MONGODB_URI;

let client;
let clientPromise;

if (!global._mongoClientPromise) {
  client = new MongoClient(uri);
  global._mongoClientPromise = client.connect();
}

clientPromise = global._mongoClientPromise;

export default async function handler(req, res) {
  const { item_id } = req.query;

  try {
    const client = await clientPromise;
    const db = client.db("game_db");

    const weapon = await db.collection("weapons").findOne({
      item_id: parseInt(item_id),
    });

    if (!weapon) {
      return res.status(404).json({ message: "Weapon not found" });
    }

    res.status(200).json(weapon);
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: "DB error" });
  }
}