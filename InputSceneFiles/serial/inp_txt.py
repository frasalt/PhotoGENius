
for i in range(0,40):
	f = open("input"+'%03d'%i+".txt", "w")
	x = 0
	y = i/10 - 1.414 
	z = -y*y+3
	f.write(
	"material sky_material(       diffuse(uniform(<0, 0, 0>)),          uniform(<1, 1, 1>)    )\n"+
	"material ground_material(    diffuse(checkered(<0.3, 0.5, 0.1>, <0.1, 0.2, 0.5>, 4)),    uniform(<0, 0, 0>)    )\n"
	"material mirror_material(    specular(uniform(<1.5, 0.2, 0.2>)),   uniform(<0, 0, 0>)    )\n"
	" \n"
	"sphere (sky_material, translation([0, 0, 0.4]) * scaling([200, 200, 200]))\n"
	"plane (ground_material, identity)\n"
	"sphere (mirror_material, translation(["+str(x)+"," +str(y)+", "+str(z)+"]))\n"
	#"sphere (mirror_material, translation([-2," +str(-i*0.1)+", 1]))\n"
	" \n"
	"camera(perspective, rotation_y(17)*translation([-5, 0, 2]), 1.0, 1.0)\n"
	)
	f.close()
