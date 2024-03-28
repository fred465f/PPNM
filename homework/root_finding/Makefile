all : Out.testRootFinder.txt Out.shootingMethodTest.gnuplot.svg Out.shootingMethodConvergenceAcc.gnuplot.svg Out.shootingMethodConvergenceEps.gnuplot.svg Out.shootingMethodConvergenceRmin.gnuplot.svg Out.shootingMethodConvergenceRmax.gnuplot.svg

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/Calculus/
LIBSPATH3 = ../../Libraries/DataStructures/

# Images.
Out.shootingMethodTest.gnuplot.svg : Out.shootingMethodTestData.txt
	echo ' \
		set terminal svg background "white" ;\
		set out "$@" ;\
		set datafile separator "," ;\
		set xlabel "r" font ", 14" ;\
		set ylabel "Prob. amp." font ", 14" ;\
		set title "GS s-wave radial wave-function of H-atom" font ", 14" ;\
		set key box width -2 height 1 font ", 14" at screen 0.9, 0.82;\
		set grid ;\
		f(x) = 0 ;\
		plot "$<" using 1:2 with lines lw 2 lc rgb "#0308B494F" title "Numerical", \
		"$<" using 1:3 with lines lw 2 lc rgb "#E03D4B" title "Analytical", \
		f(x) with lines lw 2 lc rgb "#050000000" dashtype 2 notitle ;\
	' | gnuplot
Out.shootingMethodConvergenceAcc.gnuplot.svg : Out.shootingMethodConvergenceData.txt
	echo ' \
		set terminal svg background "white" ;\
		set out "$@" ;\
		set datafile separator "," ;\
		set xlabel "Absolute error of ODE integrator" font ", 14" ;\
		set ylabel "GSE" font ", 14" ;\
		set grid ;\
		f(x) = 0 ;\
		plot "$<" using 1:2 with lines lw 2 lc rgb "#0308B494F" notitle ;\
	' | gnuplot
Out.shootingMethodConvergenceEps.gnuplot.svg : Out.shootingMethodConvergenceData.txt
	echo ' \
		set terminal svg background "white" ;\
		set out "$@" ;\
		set datafile separator "," ;\
		set xlabel "Relative error of ODE integrator" font ", 14" ;\
		set ylabel "GSE" font ", 14" ;\
		set grid ;\
		f(x) = 0 ;\
		plot "$<" using 3:4 with lines lw 2 lc rgb "#0308B494F" notitle ;\
	' | gnuplot
Out.shootingMethodConvergenceRmin.gnuplot.svg : Out.shootingMethodConvergenceData.txt
	echo ' \
		set terminal svg background "white" ;\
		set out "$@" ;\
		set datafile separator "," ;\
		set xlabel "Lower integration bound (in units of Bohr radii)" font ", 14" ;\
		set ylabel "GSE" font ", 14" ;\
		set grid ;\
		f(x) = 0 ;\
		plot "$<" using 5:6 with lines lw 2 lc rgb "#0308B494F" notitle ;\
	' | gnuplot
Out.shootingMethodConvergenceRmax.gnuplot.svg : Out.shootingMethodConvergenceData.txt
	echo ' \
		set terminal svg background "white" ;\
		set out "$@" ;\
		set datafile separator "," ;\
		set xlabel "Upper integration bound (in units of Bohr radii)" font ", 14" ;\
		set ylabel "GSE" font ", 14" ;\
		set grid ;\
		f(x) = 0 ;\
		plot "$<" using 7:8 with lines lw 2 lc rgb "#0308B494F" notitle ;\
	' | gnuplot

# Data.
Out.shootingMethodTestData.txt : shootingMethodTest.exe
	mono $< > $@
Out.shootingMethodConvergenceData.txt : shootingMethodConvergence.exe
	mono $< > $@

# Main commandline output.
Out.testRootFinder.txt : testRootFinder.exe
	mono $< > $@

# Executables.
testRootFinder.exe : testRootFinder.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
shootingMethodTest.exe : shootingMethodTest.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
shootingMethodConvergence.exe : shootingMethodConvergence.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH1)QRGS.cs $(LIBSPATH1)Splines.cs $(LIBSPATH2)RootFinder.cs $(LIBSPATH2)ODE.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll