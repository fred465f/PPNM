all : Out.testMonteCarloIntegrator.txt Out.testIntegratePlain.gnuplot.svg Out.testIntegrateQRSData.txt

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/Calculus/

# Images.
Out.testIntegratePlain.gnuplot.svg : Out.testIntegratePlainData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "N" font ", 14" ;\
			set ylabel "Error" font ", 14" ;\
			set grid ;\
			f(x) = 1/sqrt(x) ;\
			plot "$<" using 1:2 with lines lw 2 lc rgb "#36CDE0" title "Actual error", \
			"$<" using 1:3 with lines lw 2 lc rgb "#E08A48" title "Estimated error", \
			[x=50:50000] f(x) with lines lw 2 dashtype 2 lc rgb "#485E61" title "1/√N Guideline" ;\
		' | gnuplot

# Data.
Out.testIntegratePlainData.txt : testIntegratePlain.exe
	mono $< > $@
Out.testIntegrateQRSData.txt : testIntegrateQRS.exe
	mono $< > $@

# Main commandline output.
Out.testMonteCarloIntegrator.txt : testMonteCarloIntegrator.exe
	mono $< > $@

# Executables.
testMonteCarloIntegrator.exe : testMonteCarloIntegrator.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
testIntegratePlain.exe : testIntegratePlain.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
testIntegrateQRS.exe : testIntegrateQRS.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH2)MonteCarloIntegrator.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll