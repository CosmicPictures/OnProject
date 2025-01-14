// Place this file into a directory called Editor inside of your Assets directory.
 
// Models imported after you do that will have:
// - Smoothing Angle set to 180.
// - Normals set to Import.
// - Tangents set to Calculate.
// - Tangents set to Split Tangents.
// - Set material search to the whole project
 
// Any models that are already imported can have this applied by selecting them in
// the Project panel, right clicking and selecting "Reimport" from the pop-op menu.
 
// These are the settings required for using tangent space maps generated by:
// - UnityTSpace xNormal Plugin
// - www.farfarer.com/blog/2012/06/12/unity3d-tangent-basis-plugin-xnormal/
// - Handplane3D's Unity output
// - www.handplane3d.com
 
class SmoothingAngleFix extends AssetPostprocessor {
  function OnPreprocessModel () {
		var modelImporter : ModelImporter = (assetImporter as ModelImporter);
 
		// Set Smoothing Angle to 180.
		modelImporter.normalImportMode = ModelImporterTangentSpaceMode.Calculate;
		modelImporter.normalSmoothingAngle = 180.0;
 
		// Set Normals to Import.
		modelImporter.normalImportMode = ModelImporterTangentSpaceMode.Import;
 
		// Set Tangents to Calculate.
		modelImporter.tangentImportMode = ModelImporterTangentSpaceMode.Calculate;
 
		// Set Split Tangents to True.
		modelImporter.splitTangentsAcrossSeams = true;
		
		// Set material search to the whole project.
		modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;
	}
}