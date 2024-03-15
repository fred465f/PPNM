all : Out.testAdaptiveIntegrator.txt Out.testErrorFunction.gnuplot.svg

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/Calculus/

# Images.
Out.testErrorFunction.gnuplot.svg : Out.testErrorFunctionData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "erf(x)" font ", 14" ;\
			set grid ;\
			set yrange [-1.1: 1.1] ;\
			set xrange [-3.3: 3.3] ;\
			plot "$<" with lines lw 2 lc rgb "red" notitle ;\
		' | gnuplot

# Data.
Out.testErrorFunctionData.txt : testErrorFunction.exe
	mono $< > $@

# Main commandline output.
Out.testAdaptiveIntegrator.txt : testAdaptiveIntegrator.exe
	mono $< > $@

# Executables.
testAdaptiveIntegrator.exe : testAdaptiveIntegrator.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
testErrorFunction.exe : testErrorFunction.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH1)EVD.cs $(LIBSPATH2)AdaptiveIntegrator.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll