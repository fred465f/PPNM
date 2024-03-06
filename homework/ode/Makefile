all : Out.debugData.txt Out.oscillatorWithFrictionData.txt Out.lotkaVolterraSystemData.txt Out.threeBodyGravProblemData.txt

# Path to self-made LinearAlgebra library.
LIBSPATH = ../../Libraries/LinearAlgebra/

# Images.
# ...

# Data.
Out.debugData.txt : debug.exe
	mono $< > $@
Out.oscillatorWithFrictionData.txt : oscillatorWithFriction.exe
	mono $< > $@
Out.lotkaVolterraSystemData.txt : lotkaVolterraSystem.exe
	mono $< > $@
Out.threeBodyGravProblemData.txt : threeBodyGravProblem.exe
	mono $< > $@

# Executables.
debug.exe : debug.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
oscillatorWithFriction.exe : oscillatorWithFriction.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
lotkaVolterraSystem.exe : lotkaVolterraSystem.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
threeBodyGravProblem.exe : threeBodyGravProblem.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)Matrix.cs $(LIBSPATH)Vector.cs $(LIBSPATH)ODE.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll