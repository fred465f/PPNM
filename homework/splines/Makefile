all: Out.linearSpline.gnuplot.svg Out.cubicSpline.gnuplot.svg Out.quadraticSplineTestResults.txt Out.tridiagonalSolverTestResults.txt

# Path to external self-made libraries.
LIBSPATH = ../../Libraries/

# Image files.
Out.linearSpline.gnuplot.svg : Out.linearSplineData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set label "Analytical integral approx. 10.667" at -0.8, 1.5 font ", 14" ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "f(x)" font ", 14" ;\
			set title "Test of LinearSpline class" font ", 14" ;\
			set grid ;\
			set key box height 1 width -7 font ", 14" at screen 0.90, 0.82 ;\
			set xrange [-2: 2] ;\
			set yrange [-1: 7] ;\
			g(x) = 0 ;\
			plot "$<" using 1:2 with filledcurves y1=0 lc rgb "#180E8C968" title "Integral = 10.5", \
				"$<" with lines lw 2 lc rgb "#EB8628" title "Linear splines", \
				g(x) with lines lw 2 lc 0 dashtype 2 notitle, \
				"In.linearSplineData.txt" lt 7 lc rgb "#EB472D" title "Data: f(x) = 4 - x^2" ;\
		' | gnuplot
Out.cubicSpline.gnuplot.svg : Out.cubicSplineData.txt Makefile
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "y" font ", 14" ;\
			set title "Test of CubicSpline class" font ", 14" ;\
			set grid ;\
			set key box height 1 width -4 font ", 14" at screen 0.71, 0.29 ;\
			set yrange [-0.4: 1.2] ;\
			plot "$<" with lines lw 2 lc rgb "#EB8628" title "Cubic splines", \
				"Out.randomDataSet.txt" lt 7 lc rgb "#EB472D" title "Random data points" ;\
		' | gnuplot

# Data
Out.linearSplineData.txt : testLinearSpline.exe
	mono $< -inFile:In.linearSplineData.txt > $@
Out.cubicSplineData.txt : testCubicSpline.exe
	mono $< -outFile:Out.randomDataSet.txt > $@

# Main output to commandline
Out.quadraticSplineTestResults.txt : testQuadraticSpline.exe
	mono $< > $@
Out.tridiagonalSolverTestResults.txt : testTridiagonalSolver.exe
	mono $< > $@

# Executables.
testLinearSpline.exe : testLinearSpline.cs LinearAlgebra.dll splines.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:splines.dll -out:$@ $<
testQuadraticSpline.exe : testQuadraticSpline.cs LinearAlgebra.dll splines.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:splines.dll -out:$@ $<
testCubicSpline.exe : testCubicSpline.cs LinearAlgebra.dll splines.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:splines.dll -out:$@ $<
testTridiagonalSolver.exe : testTridiagonalSolver.cs LinearAlgebra.dll splines.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:splines.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)LinearAlgebra.cs
	mcs -target:library -out:$@ $<
splines.dll : splines.cs LinearAlgebra.dll
	mcs -target:library -reference:LinearAlgebra.dll -out:$@ $<

# Rule to clean.
clean :
	$(RM) *.exe [Oo]ut.* *.dll