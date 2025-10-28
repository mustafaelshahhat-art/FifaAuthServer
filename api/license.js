export default function handler(req, res) {
  res.status(200).json({
    message: "FifaAuthServer API is online ✅",
    method: req.method,
    time: new Date().toISOString()
  });
}
