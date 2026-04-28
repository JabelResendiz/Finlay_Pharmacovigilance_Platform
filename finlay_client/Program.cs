using Manual;

await User.RegisterAdmin();
await SeedCatalog.Run();

await SectionResponsibleSeed.Run();
await MedicalReviewerSeed.Run();
