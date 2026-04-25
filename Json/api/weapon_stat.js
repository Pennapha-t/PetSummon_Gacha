export default function handler(req, res) {
    const { item_id } = req.query;

    const data = {
        item_id: parseInt(item_id),
        name: "Wood Sword",
        damage: 85,
        sharpness: 10
    };

    res.status(200).json(data);
}