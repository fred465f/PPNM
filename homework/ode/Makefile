all : Out.debug.gnuplot.svg Out.oscillatorWithFriction.gnuplot.svg Out.lotkaVolterraSystemData.txt Out.equatorialMotion.gnuplot.svg Out.threeBodyGravProblemData.txt

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
Out.oscillatorWithFriction.gnuplot.svg : Out.oscillatorWithFrictionData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "Time (s)" font ", 14" ;\
			set ylabel "Angle" font ", 14" ;\
			set title "Oscillator with friction" font ", 14" ;\
			set grid ;\
			set yrange [-3.4: 3.4] ;\
			set xrange [0: 12] ;\
			plot "$<" with lines lw 2 lc rgb "#EB8628" notitle, ;\
		' | gnuplot
Out.equatorialMotion.gnuplot.svg : Out.equatorialMotionData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "y" font ", 14" ;\
			set title "Equatorial motion" font ", 14" ;\
			set grid ;\
			set xrange [-2.5: 2.5] ;\
			set yrange [-1.5: 2.5] ;\
			set key box width -7 ;\
			plot "$<" using (1/$$6)*cos($$5):(1/$$6)*sin($$5) with lines lw 2 lc rgb "#A08788" title "Relativistic prec.", \
			"$<" using (1/$$2)*cos($$1):(1/$$2)*sin($$1) with lines lw 2 lc rgb "#75080B" title "Circular motion", \
			"$<" using (1/$$4)*cos($$3):(1/$$4)*sin($$3) with lines lw 2 lc rgb "#F57D81" title "Elliptical motion", \
			"<echo 0,0" with points lt 7 lc rgb "black" title "Origin" ;\
		' | gnuplot

# Data.
Out.debugData.txt : debug.exe
	mono $< > $@
Out.oscillatorWithFrictionData.txt : oscillatorWithFriction.exe
	mono $< > $@
Out.lotkaVolterraSystemData.txt : lotkaVolterraSystem.exe
	mono $< > $@
Out.equatorialMotionData.txt : equatorialMotion.exe
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
equatorialMotion.exe : equatorialMotion.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
threeBodyGravProblem.exe : threeBodyGravProblem.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH2)ODE.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll