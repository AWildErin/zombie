"""
Takes all the source files (smd, fbx, dmx, obj) in a folder and creates a VMDL one directory up.

THIS IS VERY MESSY IM SORRY :(
"""

import sys, os
from pathlib import Path

bDebug = False

path = "D:/Steam Games/steamapps/common/sbox/addons/zombie/models/polygoncity/environment/source/"
addonName = "zombie"

# If we exported from UE4, set this to true
bUseUe4ColMesh = True

bUseDefaultMaterialGroup = True

# Polygon assets, like cars, can have upto 3 skins plus default
# Here we can tell our generator to add them.
bUseSkins = False
skinNumber = 3

bUseScaleModifier = True
scaleAmount = 0.44

def CreateVmdlFile(srcFile):
    print("Creating " + os.path.split(srcFile)[1])
    vmdlDir = os.path.abspath(os.path.join(os.path.split(srcFile)[0], os.pardir))

    fileName = os.path.splitext(srcFile)[0]
    fileName = os.path.split(fileName)[1].strip('SM_').lower() + ".vmdl"

    if (bDebug == False):
        if (os.path.isfile(vmdlDir + "/" + fileName)):
            print("Skipping " + os.path.split(srcFile)[1] + " as it already exists.")
    
    print("Writing to " + vmdlDir + "/" + fileName)
    vmdl = open(vmdlDir + "/" + fileName, "a")
    vmdl.truncate(0)

    header = '''<!-- kv3 encoding:text:version{e21c7f3c-8a33-41c5-9977-a76d3a32aa0d} format:modeldoc29:version{3cec427c-1b0e-4d48-a90a-0436f33a6041} -->
{
	rootNode = 
	{
		_class = "RootNode"
		children = 
		['''
    vmdl.write(header + "\n")

    if (bUseScaleModifier == True):
        scaleModifier = '''			{
				_class = "ModelModifierList"
				children = 
				[
					{
						_class = "ModelModifier_ScaleAndMirror"
						scale = '''+ str(scaleAmount) +'''
						mirror_x = false
						mirror_y = false
						mirror_z = false
						flip_bone_forward = false
						swap_left_and_right_bones = false
					},
				]
			},'''
        vmdl.write(scaleModifier + "\n")

    if (bUseDefaultMaterialGroup == True or bUseSkins == True):
        materialGroupListHeader = '''			{
				_class = "MaterialGroupList"
				children = 
				['''
        materialGroupListFooter = '''				]
			},'''

        materialGroupList = "\n".join([materialGroupListHeader]) + "\n"

        if (bUseDefaultMaterialGroup == True):
            defaultMaterialGroup = '''					{
						_class = "DefaultMaterialGroup"
						remaps = [  ]
						use_global_default = false
						global_default_material = ""
					},'''
            materialGroupList = materialGroupList + "\n".join([defaultMaterialGroup]) + "\n"

        if (bUseSkins == True):
            for i in range(skinNumber):
                i = i + 1
                skinNode = '''					{
						_class = "MaterialGroup"
						name = "Skin'''+ str(i) +'''"
						remaps = [  ]
					},'''
                materialGroupList = materialGroupList + "\n".join([skinNode]) + "\n"
        
        materialGroupList = materialGroupList + "\n".join([materialGroupListFooter])
        vmdl.write(materialGroupList + "\n")

    physicsShapeListHeader = '''			{
				_class = "PhysicsShapeList"
				children = 
				[
					{
						_class = "PhysicsHullFile"
						name = "''' + Path(srcFile).stem + '''"
						parent_bone = ""
						surface_prop = "default"
						collision_prop = "default"
						recenter_on_parent_bone = false
						offset_origin = [ 0.0, 0.0, 0.0 ]
						offset_angles = [ 0.0, 0.0, 0.0 ]
						filename = "''' + srcFile.lstrip().split("addons/" + addonName + "/")[1].lower() + '''"
						import_scale = ''' + str(scaleAmount) + '''
						faceMergeAngle = 10.0
						maxHullVertices = 0
						import_mode = "SingleHull"
						optimization_algorithm = "QEM"
                        import_filter = 
						{
							exclude_by_default = false
							exception_list =
                            ['''
                            
    physicsShapeListFooter = '''                            ]
						}
					},
				]
			},'''
    
    exceptionPhysMesh = Path(srcFile).stem

    vmdl.write(physicsShapeListHeader + "\n")
    if (bUseUe4ColMesh == True):
        vmdl.write('\t\t\t\t\t\t\t\t"' + exceptionPhysMesh + '",\n')

    vmdl.write(physicsShapeListFooter + "\n")

    # RenderMeshList
    renderMeshListHeader = '''			{
				_class = "RenderMeshList"
				children = 
				[
					{
						_class = "RenderMeshFile"
						filename =  "''' + srcFile.lstrip().split("addons/" + addonName + "/")[1].lower() + '''"
						import_translation = [ 0.0, 0.0, 0.0 ]
						import_rotation = [ 0.0, 0.0, 0.0 ]
						import_scale = 1.0
						align_origin_x_type = "None"
						align_origin_y_type = "None"
						align_origin_z_type = "None"
						parent_bone = ""
						import_filter = 
						{
							exclude_by_default = false
							exception_list =
                            ['''

    renderMeshListFooter = '''                            ]
						}
					},
				]
			},'''
    vmdl.write(renderMeshListHeader + "\n")

    exceptionRenderMesh = Path(srcFile).stem

    if (bUseUe4ColMesh == True):
        vmdl.write('\t\t\t\t\t\t\t\t"UCX_' + exceptionRenderMesh + '",\n')

    vmdl.write(renderMeshListFooter + "\n")

    footer = '''		]
		model_archetype = ""
		primary_associated_entity = ""
		anim_graph_name = ""
	}
}'''
    vmdl.write(footer)

    vmdl.close()


if __name__ == "__main__":
    print("Creating files...")

    for root, dir, files in os.walk(path):
        for f in files:
            # Convert model, UE4 weirdness
            if f.lower().endswith((".fbx", ".obj", ".smd", ".dmx")):
                CreateVmdlFile(os.path.join(root, f))

    #CreateVmdlFile("D:/Steam Games/steamapps/common/sbox/addons/zombie/models/polygoncity/props/source/SM_Prop_Aircon_01.FBX")

    print("Finished creating files")