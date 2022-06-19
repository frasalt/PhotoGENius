cyl_pos = -1.65
filename = "\"../Media/imgs_pfm/pgen_rett1.pfm\""
for i in range(0,103):
	f = open("input"+'%03d'%i+".txt", "w")
	x = 0
	y = -(i/10) + 7 
	z = -(y-2)*(y-2)+4*(y-2)
	f.write(
	"material sky_material(       diffuse(uniform(<0, 0, 0>)),          uniform(<1, 1, 1>)    )\n"+
	"material ground_material(    diffuse(checkered(<0.01, 0.01, 0.01>, <0.122, 0.235, 0.102>, 4)),    uniform(<0, 0, 0>)    )\n"
	"material sphere_material(    diffuse(image("+filename+")),   uniform(<0, 0, 0>)    )\n"
	" \n"
	"sphere (sky_material, translation([0, 0, 0.4]) * scaling([200, 200, 200]))\n"
	"plane (ground_material, identity)\n"
	"sphere (sphere_material, translation(["+str(x)+"," +str(y)+", "+str(z)+"]))\n"
    " \n"
	"material cylinder_material(    diffuse(uniform(<0.8, 0.4, 0.3>)),   uniform(<0, 0, 0>)    )\n"	
    "cylinder (cylinder_material, translation([0,"+str(cyl_pos)+",0]), 0, 0.8, 1.2, 6.3)\n"	
	" \n"
	"camera(perspective, translation([-3, 0, 1.5]), 1.0, 1.0)\n"
	"pointlight([-30, 0, 20], <1, 1, 1>, 0)\n"
	)
	f.close()
	
counter = 0
stretch = [1, 0.95, 0.90, 0.87, 0.86, 0.86, 0.87, 0.90, 0.95, 1]
scal = [1.02, 1.04, 1.05, 1.06, 1.07, 1.07, 1.06, 1.05, 1.04, 1.02]
for i in range(48, 58):
	f = open("input"+'%03d'%i+".txt", "w")
	x = 0
	y = -(i/20) - 2.4 + 7 
	z = stretch[counter]
	f.write(
	"material sky_material(       diffuse(uniform(<0, 0, 0>)),          uniform(<1, 1, 1>)    )\n"+
	"material ground_material(    diffuse(checkered(<0.01, 0.01, 0.01>, <0.122, 0.235, 0.102>, 4)),    uniform(<0, 0, 0>)    )\n"
	"material sphere_material(    diffuse(image("+filename+")),   uniform(<0, 0, 0>)    )\n"
	" \n"
	"sphere (sky_material, translation([0, 0, 0.4]) * scaling([200, 200, 200]))\n"
	"plane (ground_material, identity)\n"
	"sphere (sphere_material, scaling(["+str(scal[counter])+", "+str(scal[counter])+", "+str(stretch[counter])+"])*translation(["+str(x)+"," +str(y)+", "+str(z)+"]))\n"
	" \n"
    "material cylinder_material(    diffuse(uniform(<0.8, 0.4, 0.3>)),   uniform(<0, 0, 0>)    )\n"	
    "cylinder (cylinder_material, translation([0,"+str(cyl_pos)+",0]), 0, 0.8, 1.2, 6.3)\n"
    " \n"
	"camera(perspective, translation([-3, 0, 1.5]), 1.0, 1.0)\n"
	"pointlight([-30, 0, 20], <1, 1, 1>, 0)\n"
	)
	f.close()
	counter = counter+1
counter =0
for i in range(58,103):
	ff = open("input"+'%03d'%i+".txt", "w")
	x = 0
	y = -(i/10) + 7.45
	a = 2.02
	z = -(y+a)**2 + 4*(y+a)
	if i == 58:
	    cyl_pos = -1.73
	if i == 70:
	    cyl_pos = -1.87
	if i == 80:
	    cyl_pos = -1.8
	ff.write(
	"material sky_material(       diffuse(uniform(<0, 0, 0>)),          uniform(<1, 1, 1>)    )\n"+
	"material ground_material(    diffuse(checkered(<0.01, 0.01, 0.01>, <0.122, 0.235, 0.102>, 4)),    uniform(<0, 0, 0>)    )\n"
	"material sphere_material(    diffuse(image("+filename+")),   uniform(<0, 0, 0>)    )\n"
	" \n"
	"sphere (sky_material, translation([0, 0, 0.4]) * scaling([200, 200, 200]))\n"
	"plane (ground_material, identity)\n"
	"sphere (sphere_material, translation(["+str(x)+"," +str(y)+", "+str(z)+"]))\n"
	" \n"
	"material cylinder_material(    diffuse(uniform(<0.8, 0.4, 0.3>)),   uniform(<0, 0, 0>)    )\n"	
    "cylinder (cylinder_material, translation([0,"+str(cyl_pos)+",0]), 0, 0.8, 1.2, 6.3)\n"
	" \n"
	"camera(perspective, translation([-3, 0, 1.5]), 1.0, 1.0)\n"
	"pointlight([-30, 0, 20], <1, 1, 1>, 0)\n"
	)
	ff.close()
	
	
	
	
	
	