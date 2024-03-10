all : Out.debug.gnuplot.svg Out.oscillatorWithFrictionData.txt Out.lotkaVolterraSystemData.txt Out.threeBodyGravProblemData.txt

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/Calculus/
LIBSPATH3 = ../../Libraries/DataStructures/

# Images.
Out.debug.gnuplot.svg : Out.debugData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "y(x)" font ", 14" ;\
			set title "Test of ODE class for debugging" font ", 14" ;\
			set grid ;\
			set yrange [-1.2: 1.2] ;\
			set xrange [0: 6.28] ;\
			plot "$<" with lines lw 2 lc rgb "#EB8628" notitle, ;\
		' | gnuplot

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
debug.exe : debug.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
oscillatorWithFriction.exe : oscillatorWithFriction.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
lotkaVolterraSystem.exe : lotkaVolterraSystem.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
threeBodyGravProblem.exe : threeBodyGravProblem.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH2)ODE.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll