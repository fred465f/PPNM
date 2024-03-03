all: Out.txt Out.linearSplineData.svg

# Path to external self-made libraries.
LIBSPATH = ../../Libraries/

# Image files.
Out.linearSplineData.svg : Out.linearSplineData.txt Makefile
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
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

# Data
Out.linearSplineData.txt : testLinearSpline.exe
	mono $< -inFile:In.linearSplineData.txt > $@
Out.quadraticSplineData.txt : testQuadraticSpline.exe
	mono $< -inFile:In.quadraticSplineData.txt > $@

# Main output to commandline
Out.txt : main.exe
	mono $< > $@

# Executables.
main.exe : main.cs LinearAlgebra.dll splines.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:splines.dll -out:$@ $<
testLinearSpline.exe : testLinearSpline.cs LinearAlgebra.dll splines.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:splines.dll -out:$@ $<
testQuadraticSpline.exe : testQuadraticSpline.cs LinearAlgebra.dll splines.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:splines.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)LinearAlgebra.cs
	mcs -target:library -out:$@ $<
splines.dll : splines.cs LinearAlgebra.dll
	mcs -target:library -reference:LinearAlgebra.dll -out:$@ $<

# Rule to clean.
clean :
	$(RM) *.exe [Oo]ut.* *.dll