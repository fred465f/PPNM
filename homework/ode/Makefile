all : Out.debug.gnuplot.svg Out.oscillatorWithFriction.gnuplot.svg Out.lotkaVolterraSystem.gnuplot.svg Out.equatorialMotion.gnuplot.svg Out.threeBodyGravProblem.gnuplot.svg

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
			plot "$<" with lines lw 2 lc rgb "#EB8628" notitle ;\
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
			plot "$<" with lines lw 2 lc rgb "#EB8628" notitle ;\
		' | gnuplot
Out.lotkaVolterraSystem.gnuplot.svg : Out.lotkaVolterraSystemData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "Time (s)" font ", 14" ;\
			set title "Lotka-Volterra System" font ", 14" ;\
			set grid ;\
			set yrange [-0.5: 14.5] ;\
			set xrange [0: 15] ;\
			set key box width -0.6 height 1 font ", 14" at screen 0.53, 0.82 ;\
			plot "$<" using ($$1):($$2) with lines lw 2 lc rgb "#75080B" title "x(t)", \
			"$<" using ($$1):($$3) with lines lw 2 lc rgb "#F57D81" title "y(t)" ;\
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
Out.threeBodyGravProblem.gnuplot.svg : Out.threeBodyGravProblemData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "y" font ", 14" ;\
			set title "Stable planar solution to the Three-Body problem" font ", 14" ;\
			set grid ;\
			set xrange [-1.5: 1.5] ;\
			set yrange [-0.6: 0.8] ;\
			set key box width -0.9 height 0.5 font ", 14" at screen 0.65, 0.82 ;\
			plot "$<" using ($$8):($$9) with lines lw 2 lc rgb "#A08788" title "Planet 1", \
			"$<" using ($$10):($$11) with lines lw 2 lc rgb "#75080B" title "Planet 2", \
			"$<" using ($$12):($$13) with lines lw 2 lc rgb "#F57D81" title "Planet 3" ;\
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
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH1)Splines.cs $(LIBSPATH2)ODE.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll