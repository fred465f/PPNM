all : Out.debugData.txt Out.oscillatorWithFrictionData.txt Out.lotkaVolterraSystemData.txt Out.threeBodyGravProblemData.txt

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/Calculus/
LIBSPATH3 = ../../Libraries/DataStructures/

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
debug.exe : debug.cs LinearAlgebra.dll Calculus.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:Calculus.dll -out:$@ $<
oscillatorWithFriction.exe : oscillatorWithFriction.cs LinearAlgebra.dll Calculus.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:Calculus.dll -out:$@ $<
lotkaVolterraSystem.exe : lotkaVolterraSystem.cs LinearAlgebra.dll Calculus.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:Calculus.dll -out:$@ $<
threeBodyGravProblem.exe : threeBodyGravProblem.cs LinearAlgebra.dll Calculus.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:Calculus.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs
	mcs -target:library -out:$@ $^
Calculus.dll : $(LIBSPATH1)Vector.cs $(LIBSPATH2)ODE.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll