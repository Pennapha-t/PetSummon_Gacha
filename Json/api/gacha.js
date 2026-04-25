export default async function handler(req, res) {
  const weights = [
    { rarity: "C", weight: 80 },
    { rarity: "R", weight: 60 },
    { rarity: "SR", weight: 18 },
    { rarity: "SSR", weight: 1 }
  ];

  const catPool = [
    { rarity: "C", name: "Munchkin", damage: 12 },
    { rarity: "R", name: "Sphynx", damage: 30 },
    { rarity: "R", name: "British Shorthair", damage: 35 },
    { rarity: "R", name: "Russian Blue", damage: 40 },
    { rarity: "SR", name: "Siamese", damage: 45 },
    { rarity: "SR", name: "Bengal", damage: 48 },
    { rarity: "SSR", name: "Maine Coon", damage: 50 },
    // ... ใส่ให้ครบ 10 สายพันธุ์ตามเรต
  ];

  // Logic การสุ่มแบบ Weight
  const totalWeight = weights.reduce((sum, item) => sum + item.weight, 0);
  let random = Math.random() * totalWeight;
  
  let selectedRarity = "C";
  for (const w of weights) {
    if (random < w.weight) {
      selectedRarity = w.rarity;
      break;
    }
    random -= w.weight;
  }

  // กรองแมวที่ตรงกับระดับที่สุ่มได้
  const possibleCats = catPool.filter(cat => cat.rarity === selectedRarity);
  const finalCat = possibleCats[Math.floor(Math.random() * possibleCats.length)];

  res.status(200).json({
    roll_result: selectedRarity,
    cat: finalCat
  });
}